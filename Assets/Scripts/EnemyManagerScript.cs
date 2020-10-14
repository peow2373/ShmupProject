using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour {
    public Transform brick;
    public Color[] brickColors;

    public float xSpacing, ySpacing;
    public float xOrigin, yOrigin;
    public int numRows, numColumns;

    public float speed = 2f;
    public float amplitude = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < numRows; i++) {
        //    for (int j = 0; j < numColumns; j++) {
        //        Transform go = Instantiate(brick);
        //        go.transform.parent = this.transform;
        //        
        //        Vector2 loc = new Vector2(xOrigin + (i * xSpacing), yOrigin - (j * ySpacing));
        //        go.transform.position = loc;
        //
        //        Color          c  = brickColors[j];
        //        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        //        sr.color = c;
                
        //    }
        //}
        
        float rate = UnityEngine.Random.Range(2f, 4f);
        InvokeRepeating("SpawnEnemies", 1.0f, rate);
    }

    void Update()
    {
        if (GameManagerScript.endGame)
        {
            CancelInvoke();
        }
    }

    void SpawnEnemies()
    {
        Transform go = Instantiate(brick); 
        go.transform.parent = this.transform;
        Vector2 loc = new Vector2(UnityEngine.Random.Range(10.0f, -10.0f), UnityEngine.Random.Range(4.0f, 5.0f));
        go.transform.position = loc;
    }

}
