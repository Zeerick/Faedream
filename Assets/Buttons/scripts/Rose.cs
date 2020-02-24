using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Rose : MonoBehaviour
{
    public float timer;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    void gameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    void win()
    {
        winScreen.gameObject.SetActive(true);
        Time.timeScale = 0;
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

    // Update is called once per frame
    void Update()
    {
        //Timer that counts down. Game over when it reaches 0
        timer -= Time.deltaTime;
        timerText.text = Mathf.Round(timer).ToString();
        if(timer < 0)
        {
          gameOver();
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
