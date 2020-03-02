using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Rose : MonoBehaviour
{
    private static float timerDefault = 60;
    public float timer = timerDefault;
    public int roses = 0;
    public int lives = 3;
    public Text timerText;
    public Text roseText;
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    public GameObject gameOverScreen;
    public GameObject winScreen;
    public GameObject pauseScreen;
    public treeManager Trees;

    public bool timeStarted = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    void gameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        Time.timeScale = 0;
        timeStarted = false;
    }

    public void win()
    {
        winScreen.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void add(int childrenActive, int numOfChildren)
    {
        // roses += 1;
     //   roseText.text = roses.ToString() + "/" + numOfChildren;
     roseText.text = childrenActive + "/" + numOfChildren;
    }

    void pauseGame()
    {
        if (!winScreen.activeSelf && !gameOverScreen.activeSelf)
        {
            pauseScreen.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        
    }

    void unpauseGame()
    {
        pauseScreen.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void startTimer()
    {
        timer = timerDefault;
        timeStarted = true;
    }
    
    // Update is called once per frame
    void Update()
    {

        if (timeStarted)
        {
            //Timer that counts down. Game over when it reaches 0
            timer -= Time.deltaTime;
            timerText.text = Mathf.Round(timer).ToString();
            if (timer < 0)
            {
                gameOver();
            }
        }



        //life system
        if(Input.GetKeyDown("2"))
        {
            lives -= 1;
        }

        if (lives == 2){
            life3.gameObject.SetActive(false);
        }

        if (lives == 1)
        {
            life2.gameObject.SetActive(false);
        }

        if (lives == 0)
        {
            life1.gameObject.SetActive(false);
            gameOver();
        }

        //filler code for picking up roses 
        if (Input.GetKeyDown("1"))
        {
            roses += 1;
            roseText.text = roses.ToString() + "/8";
        }
        
        //Win when all roses collected
        if (roses == 8)
        {
            win();
        }
        
        // chloes - uneccessary processing if called every frame?
//        if (Trees.GetComponent<treeManager>().checkStatus())
//        {
//            win();
//        }

        //pause menu
        if (Input.GetKeyDown("escape"))
        {
            if (!pauseScreen.activeSelf)
            {
                pauseGame();
            }
            else
            {
                unpauseGame();
            }

            
        }
    }

}
