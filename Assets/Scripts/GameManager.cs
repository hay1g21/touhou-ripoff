using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //static allows access from anywhere in code, even from other scripts

    public void Awake()
    {
        if (GameManager.instance != null)
        {
            return;
        }

        //PlayerPrefs.DeleteAll(); //deletes data use for debug

        instance = this; //assigns itself to gamemanager object in the scene
        //sceneloaded is an event which fires from scenemanager when scene is loaded
        //everytime its fired thr += makes fire every func inside event (inthiscase loadstate and some others)
        SceneManager.sceneLoaded += loadState; //runs once at start
        SceneManager.sceneLoaded += onSceneLoaded; //runs every scene
        //DontDestroyOnLoad(gameObject);
        tileMap.SetActive(false);
    }

    //contains resources for the game

    //public List<Sprite> playerSprites;

    //references to other thigns
    public Player player;
    public Boss boss;
    //public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public GameObject HUD;
    public GameObject menu;
    public GameObject tileMap;

    public Text score, lives, hiScore, bombs, power, graze, points; //Various texts.
    public Text BossName; //Name of the boss.

    public void updateValues(string type, int value)
    {
        if (type.Equals("score"))
        {
            int current = int.Parse(score.text);
            current += value;
            score.text = current.ToString();
            if (current > int.Parse(hiScore.text))
            {
                hiScore.text = score.text;
            }
        }
        else if(type.Equals("player")){
            lives.text = value.ToString();
        }
    }

    //floating text
    /*
    public void ShowText(string message, int fontSize, Color colour, Vector3 position, Vector3 motion, float duration)
    {
        //dont want ref from everywhere just in one place which is from gameman, all ref from gameman
        floatingTextManager.Show(message, fontSize, colour, position, motion, duration);
    }
    */
    //upgrade weapon

    private void Update()
    {

    }

    //hitpointbar
    /*
    public void onHitPointChange()
    {
        //fetch hitpoints from player
        float ratio = (float)player.hitpoint / (float)player.maxHitPoint;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }
    */
    //save and load states
    public void saveState()
    {
        //save a string with all the data you want to player prefs
        string s = "";

        s += hiScore.text + "|";
        s += "0";

        //now save, value of key is savestate
        PlayerPrefs.SetString("SaveState", s);

    }
    public void loadState(Scene s, LoadSceneMode mode)
    {
        //Debug.Log("Lol");
        SceneManager.sceneLoaded -= loadState;

        if (!PlayerPrefs.HasKey("SaveState"))
            return;
        //get string by setting value of the key. and parse by |
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');
        Debug.Log(data[0]);
        //set score which is first value
        hiScore.text = data[0];


    }

    //for healthbar gui
    public void onHealthChange()
    {
        //get ratio of maxhealth and current health :)
        float ratio = (float)boss.health / (float)boss.maxHealth;
        hitpointBar.localScale = new Vector3(ratio, 1, 1);
        if(boss.health == 0)
        {
            //hide the boss' name!
            BossName.enabled = false;
        }
    }
    public void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("WHAT");
        if(player == null && scene.buildIndex != 0){
            player.transform.position = GameObject.Find("SpawnPoint").transform.position;
        }
        
    }

}
