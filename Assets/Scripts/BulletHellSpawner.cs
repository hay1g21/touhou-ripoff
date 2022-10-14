using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellSpawner : MonoBehaviour
{

    //some variables
    public int numColumns;
    public float speed;
    public Sprite texture; //sprite varibale
    public Color color;
    public float lifetime;
    public float firerate; //how fast bullet will be shot
    public float size;
    private float angle; //angle for bullets
    public Material material;
    public float spinSpeed; //spin of object for pretty patterns
    public float time;
    public float radiusScale; //for the hitbox of the particles

    //stuff for waves
    public float waveDur = 5.0f;
    public float waveStart;
    public int wave = 0;
    private float waveDelay = 3.0f;

    public GameObject pivot; //The main part where the bullets come out of.
    public GameObject hitBox;
    public GameObject inactive; //where old shooters go
    public List<Transform> childrenList = new List<Transform>(); //this bs
    // In this example, we have a Particle System emitting green particles; we then emit and override some properties every 2 seconds.
    public ParticleSystem system;

    

    public void Awake()
    {
        summonBullets();
        waveStart = Time.time;
    }
    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        pivot.transform.rotation = Quaternion.Euler(0, 0, time*spinSpeed);
    }
    //returns the wave
    public int getWave()
    {
        return wave;
    }

    public void Update()
    {
        /*
        if(Time.time - waveStart >= waveDur)
        {
            //stop round
            
            if(Time.time - waveStart >= waveDur+waveInter)
            {
                foreach (Transform systemChild in transform)
                {
                    childrenList.Add(systemChild);
                    
                }
                foreach (Transform systemChild in childrenList)
                {
                    systemChild.parent = inactive.transform;
                }
                
                //start next wave
                nextWave();
                waveStart = Time.time;
            }
            
        }
        */
    }
    void summonBullets()
    {
       
        //first clear bullets

        //this for loop creates multiple particle systems, imagine it as each line of bullets
        for (int i = 0; i < numColumns; i++)
        {
            //give the angle step for the particle system with this formula
            angle = 360f / numColumns;

            // A simple particle material with no texture.
            Material particleMaterial = material; //new Material(Shader.Find("Particles/Standard Unlit"));

            // Create a green Particle System.
            var go = new GameObject("Particle System");
            go.transform.Rotate(angle*i, 90, 0); // Rotate system orientation, particles would be invisible otherwise
            go.transform.parent = this.pivot.transform; //gives a parent to the part system, can be anything (a pivot)
            go.transform.position = this.pivot.transform.position; //starts at position of the object
            system = go.AddComponent<ParticleSystem>();
            go.GetComponent<ParticleSystemRenderer>().material = particleMaterial;
            var mainModule = system.main;
            mainModule.startSpeed = speed;
            mainModule.maxParticles = 100000;
            mainModule.simulationSpace = ParticleSystemSimulationSpace.World; //makes it so particles behave differently (world for independent to object)
            var emission = system.emission; //gets the emission module (property sidebar in inspector)
            emission.enabled = false; //disables main emission of particle emitter

            //stuff for shape system
            var shape = system.shape;
            shape.enabled = true; //enables shape of particles, allows change of particle formation
            shape.shapeType = ParticleSystemShapeType.Sprite; //leave this null for a straight line
            shape.sprite = null;

            // stuff for texture system
            var texSys = system.textureSheetAnimation;
            texSys.enabled = true;
            texSys.mode = ParticleSystemAnimationMode.Sprites;
            texSys.AddSprite(texture);

            //stuff for trigger
            var trig = system.trigger;
            trig.enabled = true;
            trig.AddCollider(hitBox.GetComponent<BoxCollider2D>());
            trig.inside = ParticleSystemOverlapAction.Callback;
            trig.enter = ParticleSystemOverlapAction.Callback;
            trig.exit = ParticleSystemOverlapAction.Callback;
            //go.AddComponent<BulletTrigger>(); //adds a bullet trigger script which we may not need, disabled!

            //collision detection (may be wonky but lets see if we can put some magic into it)
            var coll = system.collision;
            coll.type = ParticleSystemCollisionType.World; //collision is based in the world?? no idea
            coll.mode = ParticleSystemCollisionMode.Collision2D; //think this is it
            coll.collidesWith = LayerMask.GetMask("Nothing");
            coll.collidesWith = LayerMask.GetMask("Player"); //have to get player layer for this one
            coll.enabled = true;
            coll.lifetimeLoss = 1; //make them disappear after collision;
            coll.radiusScale = radiusScale; //changes hitbox to make them smaller/bigger than sprite, useful
            coll.sendCollisionMessages = true; //feedback to the scripts i think lol

            //katerina did this part
            go.AddComponent<BulletCollision>(); //adds amazing bullet collision to magical bullets! 
        }

        

        // Every 2 secs we will emit.
        InvokeRepeating("DoEmit", 0f, firerate);


    }

    void DoEmit()
    {
        //make each particle system shoot particles
        foreach(Transform systemChild in pivot.transform)
        {
            //modify system variable to be for each of the children, otherwise it will refer to one of them
            system = systemChild.GetComponent<ParticleSystem>();
            // Any parameters we assign in emitParams will override the current system's when we call Emit.
            // Here we will override the start color and size.
            var emitParams = new ParticleSystem.EmitParams();
            emitParams.startColor = color;
            emitParams.startSize = size;
            emitParams.startLifetime = lifetime;
            system.Emit(emitParams, 1);
        }

        
        //system.Play(); // Continue normal emissions
    }

    /*can change these vars
     * color
     * numcolumns
     * size
     * speed
     * firerate
     * spinspeed
     * lifetime
     * wavedur
     */
    public void clearBullets()
    {
        foreach (Transform systemChild in pivot.transform)
        {
            childrenList.Add(systemChild);

        }
        foreach (Transform systemChild in childrenList)
        {
            systemChild.parent = inactive.transform; //Remove this.

            //clearing bullets please :(
            systemChild.GetComponent<ParticleSystem>().Clear(); //Clears bullets.
            Destroy(systemChild.gameObject);
        }

        //clear them every stage, looks like it works
        childrenList = new List<Transform>();
    }
    public void nextWave()
    {
        //cancels shooting and clears all the bullets
        //CancelInvoke();
        //clearBullets();

        //increase wave
        wave++;
        Debug.Log("Next wave: wave "+ wave.ToString());
        if(wave == 1)
        {
            spinSpeed = 10.0f;
            color = Color.green;
            numColumns = 20;
            speed = 0.4f;
            lifetime = 20.0f;
            firerate = 0.2f;
            size = 0.05f;
            
            Invoke("summonBullets",waveDelay);

        }
        if (wave == 2)
        {
            spinSpeed = 50.0f;
            color = Color.cyan;
            numColumns = 10;
            size = 0.75f;
            speed = 1.0f;
            firerate = 1.0f;
            lifetime = 10.0f;
            waveDur = 5.0f;
            radiusScale = 0.2f;
            Invoke("summonBullets", waveDelay);
        }
        if(wave == 3)
        {
            spinSpeed = 160.0f;
            color = Color.magenta;
            numColumns = 20;
            size = 0.1f;
            lifetime = 10.0f;
            firerate = 0.10f;
            waveDur = 11.0f;
            speed = 2.0f;
            Invoke("summonBullets", waveDelay);
            //wave = 0;
        }
     
    }
}
