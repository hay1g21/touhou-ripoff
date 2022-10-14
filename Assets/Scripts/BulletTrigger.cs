using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrigger : MonoBehaviour
{
    //script for when the player touches one of the bullets
    ParticleSystem partSys;
    Player player;
    Boss boss;
    Enemy enemy;
    // these lists are used to contain the particles which match
    // the trigger conditions each frame.
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

    private void Start()
    {
        get();
    }
    void OnEnable()
    {
        partSys = GetComponent<ParticleSystem>();
        //store object containing the partsystem
       
    }

    void OnParticleTrigger()
    {
        //for all particles, check if they have collided with any object with the player tag
        player = GameManager.instance.player;
        boss = GameManager.instance.boss;
        BoxCollider2D collider = player.hitBox.GetComponent<BoxCollider2D>();
        // get the particles which matched the trigger conditions this frame
        int numEnter = partSys.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        int numExit = partSys.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
        int numInside = partSys.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
       // ParticleSystem.Particle[] particles = new ParticleSystem.Particle[partSys.particleCount];
        //int num = partSys.GetParticles(particles);
        //Debug.Log("help");
        // iterate through the particles which entered the trigger and make them red
        for (int i = 0; i < numEnter; i++)
        {
            GameObject owner = partSys.transform.parent.gameObject;
            if(owner.tag == "Enemy")
            {
                ParticleSystem.Particle p = enter[i];
                p.startColor = new Color32(255, 0, 0, 255);
                enter[i] = p;

                Debug.Log(owner);

                player.death();
            }
            if(owner.tag == "Player")
            {
                ParticleSystem.Particle p = enter[i];
                p.startColor = new Color32(255, 0, 0, 255);
                boss.takeDamage(player.getDamage());
                GameManager.instance.updateValues("score", 50);
                p.remainingLifetime = 0;
                enter[i] = p;
                //set partsys again?

                // *pass* this array to GetParticles...


            }
            
        }
        //partSys.SetParticles(particles, num);

        // iterate through the particles which exited the trigger and make them green
        /*
        for (int i = 0; i < numExit; i++)
        {
            ParticleSystem.Particle p = exit[i];
            p.startColor = new Color32(0, 255, 0, 255);
            exit[i] = p;
        }
        */

        // re-assign the modified particles back into the particle system
        partSys.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        partSys.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
    }

    public void get()
    {
        List<int> list = new List<int>();
        list.Add(2);
        list.Add(3);
        list.Add(7);

        Debug.Log(list.Count);
    }
}
