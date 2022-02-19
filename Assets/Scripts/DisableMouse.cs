using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMouse : MonoBehaviour
{
    GameObject lastselect;
    void Start()
    {
        lastselect = new GameObject();
    }

    /*
    void Update()
    {
        if (e.GetComponent<EventSystem>.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }
        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
    }
    */
}
