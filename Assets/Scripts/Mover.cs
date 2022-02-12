using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour //abstracted not able to drop on object, must be inherited
{
    //declare variables

    private Vector3 originalSize;
    //protected RaycastHit2D hit;
    protected Vector3 moveDelta;

    public float ySpeed = 1.0f;
    public float xSpeed = 1.0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        originalSize = transform.localScale;
     
    }
    
    protected virtual void UpdateMotor(Vector3 input)
    {
        //reset movedelta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);



        //swap sprite direction based on movement
        if (moveDelta.x > 0)
        {
            transform.localScale = originalSize;
        }
        else if (moveDelta.x < 0)
        {
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);
        }
  
        //move x
        transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);

        
        //move y
        transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
     
    }
}
