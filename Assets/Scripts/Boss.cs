using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy
{
    //need this for phase changes
    public BulletHellSpawner bulletSpawner;

    public List<int> healthPhases;
    public bool changingPhase = false;
    public float delay = 2f;
    private float timeOfDeath; //time when boss is damaged
    // Update is called once per frame
    
    private Transform rotationCenter;

    //sfx
    public AudioSource phaseClear; //when boss runs out of health and bullets are cleared
    public AudioSource phaseStart; //when a new spell of bullets starts

    //animation stuff
    public GameObject spellImage; //the image that shows during a spellcard
    public GameObject spellText; //the text that appears from a spellcard

    public string[] spellcards = new string[5];//storing spells
    private int count = 0;

    private Animator spellAnimator; //animator for spellcards
    private Animator textAnimator; //animator for text

    const string IMAGE_MOVE = "imageMove";
    const string IDLE = "idle";

    const string TEXT_SHOW = "spellTextShow";
    const string TEXT_IDLE = "spellTextIdle";
    const string TEXT_HIDE = "spellTextHide";

    private string currentState;
    private string currentTextState;

    public void Start()
    {
        maxHealth = healthPhases[0];
        health = maxHealth;
        bulletSpawner = gameObject.GetComponent<BulletHellSpawner>();
        Debug.Log(healthPhases.Count);
        rotationCenter = new GameObject().GetComponent<Transform>();
        rotationCenter.parent = gameObject.transform;
        rotationCenter.position = gameObject.transform.position;
        rotationCenter.name = "RotationCenter";
        timeOfDeath = 0f;

        //get spellcard animator
        spellAnimator = spellImage.GetComponent<Animator>();
        //get text animator
        textAnimator = spellText.GetComponent<Animator>();
    }

    public void Update()
    {
        //moveCircle();
    }

    public float animDelay = 3f; //how long the spelltransition plays for

    //for changing animations for spellls
    public void changeSpellState(string newState)
    {
        Debug.Log("Playing animation " + newState);
        //Stop the same animation interrupting itself.
        if(currentState == newState) return;

        //Play.
        spellAnimator.Play(newState);

        //Assign.
        currentState = newState;

    }

    //for changing animations for text oh god kill me
    public void changeTextState(string newState)
    {
        Debug.Log("Playing animation " + newState);
        //Stop the same animation interrupting itself.
        if (currentState == newState) return;

        //Play.
        textAnimator.Play(newState);

        //Assign.
        currentTextState = newState;

    }

    public void updateSpell()
    {
        //iterate array and change text
        
        if(count < spellcards.Length)
        {
            spellText.GetComponent<Text>().text = spellcards[count];
        }
        count++;

    }

    //sets bool back to false
    public void endTransition()
    {
        changingPhase = false;
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

    //period between phase end and phase start, breathing time
    public void interval()
    {
        //invoke to call after recovery delay
        updateSpell(); //change text
        changeSpellState(IMAGE_MOVE);
        changeTextState(TEXT_SHOW);
        phaseStart.Play();
        Invoke("makeIdle", animDelay);
        advanceWave();
        //Invoke("advanceWave", animDelay);
        
    }

    public override void takeDamage(int damageAmount)
    {
        if (!changingPhase)
        {
            health -= damageAmount; //deduct damage from health, simple stuff...
        }
        else
        {
            health -= Mathf.RoundToInt(damageAmount * 0.2f);
            
        }
        

        //health lower than 0, boss has run out of health!
        if (health <= 0)
        {
            health = 0; //don't go negative health
            changingPhase = true; //now boss is changing phase
            //need boss invincibility period

            if (Time.time - timeOfDeath > delay)
            {
                Debug.Log("Changing phase");
                if(bulletSpawner.getWave() > 0)
                {
                    //make that text disappear! Edit, make this apply to spellcards only
                    changeTextState(TEXT_HIDE);
                }
                

                phaseClear.Play(); //soouund
                                   //check if dead by matching new wave num with phases
                if (bulletSpawner.getWave() == healthPhases.Count - 1)
                {
                    //all phases gone, you must die
                    Debug.Log("Boss dedad");
                    GameManager.instance.saveState();
                    death();
                }
                else
                {
                    //clear the bullets here
                    //cancels shooting and clears all the bullets
                    bulletSpawner.CancelInvoke();
                    bulletSpawner.clearBullets();
                    //invoke the interval, for a delay
                    Invoke("interval",delay);

                }

                timeOfDeath = Time.time; //new time of death is set
            }

            
           
        }
        GameManager.instance.onHealthChange();
    }

    public void makeIdle()
    {
        //put animation back in order
        changeSpellState(IDLE);
        
    }
    //method for solely wave advancing purposes
    public void advanceWave()
    {
        
        
        //advance the wave
        bulletSpawner.nextWave();
        Invoke("endTransition", bulletSpawner.getWaveDelay()); //allows damage reduction until the bullets fire
        //get new health
        maxHealth = healthPhases[bulletSpawner.getWave()];
        health = maxHealth;
        //reset bar
        GameManager.instance.onHealthChange();
        //finish and phase can change again
        
    }

    
}
