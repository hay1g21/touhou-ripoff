using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// If there is no selected item, set the selected item to the event system's first selected item
public class ControllerRefocus : MonoBehaviour
{
   // InputSystem.DisableDevice(DisableMouse)
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            Debug.Log("Reselecting first input");
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        }
    }
}
