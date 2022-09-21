using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    public void OnParticleCollision(GameObject other)
    {
        
        Debug.Log("Collided");
    }
}
