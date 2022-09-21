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
    
    private Transform rotationCenter;

    //sfx
    public AudioSource phaseClear; //when boss runs out of health and bullets are cleared
    public AudioSource phaseStart; //when a new spell of bullets starts

    public void Start()
    {
        maxHealth = healthPhases[0];
        health = maxHealth;
        bulletSpawner = gameObject.GetComponent<BulletHellSpawner>();
        Debug.Log(healthPhases.Count);
        rotationCenter = new GameObject().GetComponent<Transform>();
        rotationCenter.position = gameObject.transform.position;
    }

    public void Update()
    {
        moveCircle();
    }

    private float angle = 0f;
    public float radius = .2f;
    public float angularSpeed = 2f;

    public void moveCircle()
    {
        transform.position = new Vector3(rotationCenter.position.x + Mathf.Cos(angle) * radius, rotationCenter.position.y + Mathf.Sin(angle) * radius, 0);
        angle = angle + Time.deltaTime * angularSpeed;

        if(angle >= 360f)
        {
            angle = 0f;
        }
        //Debug.Log("Changed to: " +rotationCenter.position.x);
    }

    public override void takeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            //switch to next wave
            health = 0;
            phaseClear.Play();
            if (!changingPhase)
            {
                Debug.Log("Changing phase");
                changingPhase = true;
                //change the wave
                bulletSpawner.nextWave();

                //then check if dead by matching new wave num with phases
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
                    phaseStart.Play();
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
