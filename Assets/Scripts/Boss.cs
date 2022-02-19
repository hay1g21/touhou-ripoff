using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    //need this for phase changes
    BulletHellSpawner bulletSpawner;

    public List<int> healthPhases;
    public bool changingPhase = false;
    // Update is called once per frame
    public void Start()
    {
        maxHealth = healthPhases[0];
        health = maxHealth;
        bulletSpawner = gameObject.GetComponent<BulletHellSpawner>();
        Debug.Log(healthPhases.Count);
    }

    public override void takeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            //switch to next wave
            health = 0;
           
            if (!changingPhase)
            {
                Debug.Log("Changing phase");
                changingPhase = true;
                //change the wave
                bulletSpawner.nextWave();
                //check if dead first
                if (bulletSpawner.getWave() == healthPhases.Count)
                {
                    //all phases gone, you must die
                    Debug.Log("Boss dedad");
                    GameManager.instance.saveState();
                    death(); 
                }
                else
                {
                    //get new health
                    maxHealth = healthPhases[bulletSpawner.getWave()];
                    health = maxHealth;
                    //reset bar
                    GameManager.instance.onHealthChange();
                    //finish and phase can change again
                    
                }
                changingPhase = false;

            }
           
        }
        GameManager.instance.onHealthChange();
    }
    /*
    public void death()
    {
        Destroy(this.gameObject);
    }
    */
}
