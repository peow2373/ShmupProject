using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static float score = 0;
    public static int lives = 5;
    public static float chargeCombo = 1;
    public static float swordCombo = 1;

    public GameObject[] hearts;
    
    // Start is called before the first frame update
    void Start()
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
        

        Debug.Log(score);
        //Debug.Log(lives);
    }
}
