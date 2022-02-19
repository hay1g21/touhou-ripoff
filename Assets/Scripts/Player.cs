using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    private SpriteRenderer spriteRen;
    private Color color;
    private SpriteRenderer hitRen;
    private Color hitColor;
    private BoxCollider2D boxCollider;
    private int numLives;
    private float timeOfDeath;
    private float respawnTime = 1.0f; //time until respawn
    private float invinPeriod = 2.0f; //stays alive for a bit
    private bool isAlive = true;

    public GameObject hitBox;
    public float focusMult; //multiplier for focus speed
    //SHOTO
    //some variables
    public int damage; //damage for bullets
    public int numColumns;
    public float bulSpeed;
    public Sprite texture; //sprite varibale
    public Color bulletColor;
    public float lifetime;
    public float firerate; //how fast bullet will be shot
    public float size;
    private float angle; //angle for bullets
    public Material material;
    public float spinSpeed; //spin of object for pretty patterns
    public float time;
    public ParticleSystem system;


    protected override void Start()
    {
        base.Start();
        createWeapon();
        Debug.Log("IsShooting");
        boxCollider = hitBox.GetComponent<BoxCollider2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        hitRen = hitBox.GetComponent<SpriteRenderer>();
        color = spriteRen.color;
        numLives = 3;
    }
    public void Awake()
    {
        //createWeapon();

    }
    private void Update()
    {
        updateBoundaries();
        shoot();
        if (!isAlive && Time.time - timeOfDeath > respawnTime)
        {
            respawn();
        }
    }
    private void FixedUpdate()
    {
        //check if outside boundaries
        
        //reset moveDelta
        float x = Input.GetAxisRaw("Horizontal"); //returns -1 or 1 for left and right
        float y = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //focus, change hitbox and speed
            
            if (isAlive)
            {
                hitColor = hitRen.color;
                hitColor.a = 1;
                hitRen.color = hitColor;
                UpdateMotor(new Vector3(x, y, 0).normalized * focusMult);
            }
            
        }
        else
        {
            hitColor = hitRen.color;
            hitColor.a = 0;
            hitRen.color = hitColor;
            //focus speed
            if (isAlive)
            {
                UpdateMotor(new Vector3(x, y, 0).normalized);
            }
            
        }
        
        
    }

    public bool shootActive = false;
    public List<Transform> childrenList = new List<Transform>(); //this bs

    void createWeapon()
    {
        
            //give the angle step for the particle system with this formula
            //angle = 360f / numColumns;

            // A simple particle material with no texture.
            Material particleMaterial = material; //new Material(Shader.Find("Particles/Standard Unlit"));

            // Create a green Particle System.
            var go = new GameObject("Particle System");
            go.transform.Rotate(270, 90, 0); // Rotate system orientation, particles would be invisible otherwise
            go.transform.parent = this.transform; //gives a parent to the part system
            go.transform.position = this.transform.position; //starts at position of the object
            system = go.AddComponent<ParticleSystem>();
            go.GetComponent<ParticleSystemRenderer>().material = particleMaterial;
            var mainModule = system.main;
            mainModule.startSpeed = bulSpeed;
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
        //trig.AddCollider(hitBox.GetComponent<BoxCollider2D>());
        Debug.Log(GameManager.instance.boss.gameObject.name);
            trig.AddCollider(GameManager.instance.boss.GetComponent<BoxCollider2D>());
            trig.inside = ParticleSystemOverlapAction.Callback;
            trig.enter = ParticleSystemOverlapAction.Callback;
            trig.exit = ParticleSystemOverlapAction.Callback;
            go.AddComponent<BulletTrigger>(); //lets particles be triggered
    }

        public void shoot()
    {
        //need particles i think for this
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (shootActive == false && isAlive)
            {
                

                InvokeRepeating("DoEmit", 0f, firerate);
                shootActive = true;
            }
            //bulletColor = Color.green;
            //spinSpeed = 10.0f;

            //if holding z shoot
            //need to hold a particle emmitter
            
        }
        else if (Input.GetKeyUp(KeyCode.Z) || !isAlive)
        {
            shootActive = false;
            CancelInvoke();

        }
    }
    //shoot the particles
    void DoEmit()
    {
        //make each particle system shoot particles
        foreach (Transform systemChild in transform)
        {
            //modify system variable to be for each of the children, otherwise it will refer to one of them
            if(systemChild.GetComponent<ParticleSystem>() != null){
                system = systemChild.GetComponent<ParticleSystem>();
                // Any parameters we assign in emitParams will override the current system's when we call Emit.
                // Here we will override the start color and size.
                var emitParams = new ParticleSystem.EmitParams();
                emitParams.startColor = bulletColor;
                emitParams.startSize = size;
                emitParams.startLifetime = lifetime;
                system.Emit(emitParams, 1);
            }
            
        }


        //system.Play(); // Continue normal emissions
    }
    //keep the player in the boundaries man
    public float rightBoundary;
    public float leftBoundary;
    public float upBoundary;
    public float downBoundary;

    public void updateBoundaries()
    {
        //restricts a value in a range
        float xClamp = Mathf.Clamp(transform.position.x, leftBoundary, rightBoundary);
        float yClamp = Mathf.Clamp(transform.position.y, downBoundary, upBoundary);
        transform.position = new Vector3(xClamp, yClamp, 0);
    }
    //called when hit by bullet
    public void death()
    {
        if(Time.time - timeOfDeath > invinPeriod)
        {
            //change isalive to false to stop from moving
            isAlive = false;

            color.a = 0;
            hitColor.a = 0;
            spriteRen.color = color;
            hitRen.color = hitColor;
            //decrease lives or gameover
            if (numLives != 0)
            {
                numLives--;
                GameManager.instance.updateValues("player", numLives);
            }
            else
            {
                //gameover
                //return;
            }
            timeOfDeath = Time.time;
        }
        
    }
    public void respawn()
    {
        color.a = 1;
        hitColor.a = 1;
        spriteRen.color = color;
        hitRen.color = hitColor;
        isAlive = true;
    }

    public int getDamage()
    {
        return damage;
    }
}
