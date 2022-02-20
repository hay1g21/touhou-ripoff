using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MenuScript
{
    public GameObject backDrop;
    public bool isPaused;

    // Start is called before the first frame update
    public void Start()
    {
        backDrop.SetActive(false);
        isPaused = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Debug.Log("WOrk");
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }
    }

    public void pauseGame()
    {
        backDrop.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void resumeGame()
    {
        
        backDrop.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log(isPaused);
    }

    //return to title screen
    public void returnTitle()
    {
        //save game
        Time.timeScale = 1f;
        GameManager.instance.saveState();
        SceneManager.LoadScene(0);
    }
}
