using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    //enter game
    public void play()
    {
        SceneManager.LoadScene(1);
    }
    
    public void exitGame()
    {
        //quits game
        Application.Quit();
    }

   

}
