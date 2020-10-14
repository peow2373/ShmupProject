using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemyManagerScript : MonoBehaviour {
    public Transform ninja;
    public Color[] brickColors;

    public float xSpacing, ySpacing;
    public float xOrigin, yOrigin;
    public int numRows, numColumns;

    public float speed = 2f;
    public float amplitude = 0.5f;
    private int oncoming = 0;

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
        
        float rate = UnityEngine.Random.Range(0.5f, 3f);
        InvokeRepeating("SpawnEnemies", 1.0f, rate);
    }

    void Update()
    {
        oncoming = UnityEngine.Random.Range(0, 2);
        
        if (GameManagerScript.endGame)
        {
            CancelInvoke();
        }
    }

    void SpawnEnemies()
    {
        if (oncoming == 0)
        {
            Transform go = Instantiate(ninja); 
            go.transform.parent = this.transform;
            Vector2 loc = new Vector2(UnityEngine.Random.Range(-11.0f, -9.0f), UnityEngine.Random.Range(-4.0f, 4.0f));
            go.transform.position = loc;
        }
        else
        {
            Transform go = Instantiate(ninja); 
            go.transform.parent = this.transform;
            Vector2 loc = new Vector2(UnityEngine.Random.Range(9.0f, 11.0f), UnityEngine.Random.Range(-4.0f, 4.0f));
            go.transform.position = loc;
        }
    }

}
