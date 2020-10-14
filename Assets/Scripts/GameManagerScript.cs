using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static float score = 0;
    public static int lives = 5;
    public static float chargeCombo = 1;
    public static float swordCombo = 1;

    public GameObject[] hearts;
    public GameObject square;
    public GameObject scoreKeeper;
    public Text scoreText;
    public Text endScore;
    public Text bestScore;
    public Text gameOver;
    public Text restart;

    public static bool endGame = false;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Sword combo
        if (GameObject.FindGameObjectsWithTag("projectile").Length > 3 && GameObject.FindGameObjectsWithTag("projectile").Length < 5)
        {
            swordCombo = 2;
        }
        else if (GameObject.FindGameObjectsWithTag("projectile").Length >= 5 && GameObject.FindGameObjectsWithTag("projectile").Length < 7)
        {
            swordCombo = 3;
        }
        else if (GameObject.FindGameObjectsWithTag("projectile").Length >= 7)
        {
            swordCombo = 4;
        }
        else
        {
            swordCombo = 1;
        }

        // Display lives remaining
        for (int i = 0; i < hearts.Length; i++)
        {
            if (lives > i)
            {
                hearts[i].gameObject.SetActive(true);
            }
            else
            {
                hearts[i].gameObject.SetActive(false);
            }
        }

        scoreText.text = "Score:" + score;

        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        endGame = true;

        square.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(true);
        if (score == HighScoreScript.highScore)
        {
            endScore.text = "Score: " + score + "!";
        }
        else
        {
            endScore.text = "Score: " + score;
        }
        bestScore.text = "High Score: " + HighScoreScript.highScore + "!";
        endScore.gameObject.SetActive(true);
        bestScore.gameObject.SetActive(true);
        restart.gameObject.SetActive(true);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            SceneManager.LoadScene("Main");
            square.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(true);
            gameOver.gameObject.SetActive(false);
            endScore.gameObject.SetActive(false);
            bestScore.gameObject.SetActive(false);
            restart.gameObject.SetActive(false);

            score = 0;
            lives = 5;
            HighScoreScript.temp = true;
            chargeCombo = 1;
            swordCombo = 1;
            endGame = false;
        }
    }
}
