using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{

    public float moveSpeed; //Speed at which the enemy moves around.
    private float moveDir; //Direction, negative for left, positive to move right.

    //some variables for creating the enemies weapon...

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
    public GameObject Pivot;

    
    public Transform target; //Player for the enemies to kiill
    public float rotationModifier;
    public Quaternion initRot;
    public float speed = 1f;

    public void Start()
    {
        health = maxHealth;
        moveDir = 1;
        target = GameManager.instance.player.transform.GetChild(0); //Get the player so it can lock on. can change
        createWeapon(); //create the weapon
        shoot(); //start blasting

    }
    public void Update()
    {
        move();
        look();
        
    }
    //A hassle, points enemy shots at the player.
    public void look()
    {
        Vector3 vectorToTarget = target.transform.position - transform.position;
        //triangle stuff
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
        //Debug.Log(angle);
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        Pivot.transform.rotation = Quaternion.Slerp(Pivot.transform.rotation, q, Time.deltaTime * speed);
    }

    //Simple bullet weapon that points towards the player.
    public void createWeapon()
    {
        //give the angle step for the particle system with this formula
        //angle = 360f / numColumns;

        // A simple particle material with no texture.
        Material particleMaterial = material; //new Material(Shader.Find("Particles/Standard Unlit"));

        // Create a green Particle System.
        var go = new GameObject("Particle System");
        go.transform.Rotate(270, 90, 0); // Rotate system orientation, particles would be invisible otherwise
        go.transform.parent = Pivot.transform;//this.transform; //gives a parent to the part system
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

        //trigger isnt needed

        //things for collision
        go.AddComponent<BulletCollision>();

        //collision detection (may be wonky but lets see if we can put some magic into it)
        var coll = system.collision;
        coll.type = ParticleSystemCollisionType.World; //collision is based in the world?? no idea
        coll.mode = ParticleSystemCollisionMode.Collision2D; //think this is it
        coll.collidesWith = LayerMask.GetMask("Nothing");
        coll.collidesWith = LayerMask.GetMask("Player"); //have to get player layer for this one
        coll.enabled = true;
        coll.lifetimeLoss = 1; //make them disappear after collision;
        coll.radiusScale = 1; //changes hitbox to make them smaller/bigger than sprite, useful
        coll.sendCollisionMessages = true; //feedback to the scripts i think lol

    }

    public void shoot()
    {
        InvokeRepeating("DoEmit", 0f, firerate); //think this repeats the shooting? It does.

        //Cancel Invoke for stopping the shots. Remember that.


        
    }

    void DoEmit()
    {
        //make each particle system shoot particles
        foreach (Transform systemChild in Pivot.transform)
        {
            //modify system variable to be for each of the children, otherwise it will refer to one of them
            if (systemChild.GetComponent<ParticleSystem>() != null)
            {
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
    }

    //move move movement testing
    public void move()
    {
        transform.position = new Vector2(transform.position.x + moveDir * moveSpeed * Time.deltaTime, transform.position.y);
        //Debug.Log(transform.position.x);

        if (transform.position.x > 3f)
        {
            
            moveDir = -1; //rotate here
        }
        if (transform.position.x < -1f)
        {
            moveDir = 1; //and again rotate here again, and avoid mistakes..
        }
    }
}
