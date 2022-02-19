using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour //abstracted not able to drop on object, must be inherited
{
    //declare variables

    private Vector3 originalSize;
    //protected RaycastHit2D hit;
    protected Vector3 moveDelta;

    public float ySpeed;
    public float xSpeed;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        originalSize = transform.localScale;
     
    }
    
    protected virtual void UpdateMotor(Vector3 input)
    {
        //reset movedelta
        moveDelta = input;
        //moveDelta = new Vector3(input.x, input.y, 0).normalized; //normalise to fix vertical speed being faster, makes x and y smaller to account for both keys being pressed
        Debug.Log(moveDelta.magnitude + " x: " + moveDelta.x + "y: " + moveDelta.y);



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
        Debug.Log(moveDelta.x * xSpeed * Time.deltaTime);

        transform.Translate(moveDelta.x * xSpeed * Time.fixedDeltaTime, 0, 0);

        
        //move y
        transform.Translate(0, moveDelta.y *ySpeed * Time.fixedDeltaTime, 0);
     
    }
}
