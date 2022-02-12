using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{


    // Update is called once per frame
    void Update()
    {
        
    }

    public override void takeDamage(int damageAmount)
    {
        base.takeDamage(damageAmount);
        GameManager.instance.onHealthChange();
    }
    /*
    public void death()
    {
        Destroy(this.gameObject);
    }
    */
}
