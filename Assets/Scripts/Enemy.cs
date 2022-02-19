using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;
    public int score; //score given upon being dead

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void takeDamage(int damageAmount)
    {

        health -= damageAmount;
        if (health <= 0)
        {
            health = 0; 
            death();
        }
    }
    public void death()
    {
        GameManager.instance.updateValues("score", score);
        Destroy(this.gameObject);
    }
}
