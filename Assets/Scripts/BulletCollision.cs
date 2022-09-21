using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    //Do it all in one place for now.

    private Player player;
    private Boss boss;
    private Enemy enemy;

    private void Start()
    {
        player = GameManager.instance.player;
        boss = GameManager.instance.boss;
    }

    public void OnParticleCollision(GameObject collided)
    {
        Debug.Log(collided + " has collided with a bullet."); //Shows the gameObject the bullet has collided with.

        //Determines what has been hit.

        if (collided.name == "Hitbox")
        {
            //Has hit witch, eliminate.
            player.death();
        }
        else if (collided.tag == "Boss")
        {
            //Hit the boss, deal damage.
            boss.takeDamage(player.getDamage());
            GameManager.instance.updateValues("score", 50);
        }
        else if (collided.tag == "Enemy")
        {
            //Hit smallfry, deal damage.
            collided.GetComponent<Enemy>().takeDamage(player.getDamage());
        }
    }
}
