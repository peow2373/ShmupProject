using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreScript : MonoBehaviour
{
    public List<float> scoreArray;
    public static float highScore = 0;
    public static bool temp = true;
    private float score = 0;
    
    public static HighScoreScript Instance;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy (gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        score = GameManagerScript.score;

        if (GameManagerScript.endGame)
        {
            if (temp)
            {
                scoreArray.Add(score);
                Debug.Log("score added");
                temp = false;
            }
            
            for (int i = 0; i < scoreArray.Count; i++)
            {
                float temporary = scoreArray[i];
                Debug.Log(temporary);

                if (temporary > highScore)
                {
                    highScore = temporary;
                }
            }
        }
    }
}
