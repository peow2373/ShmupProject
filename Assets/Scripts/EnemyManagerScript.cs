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
        
        Transform go = Instantiate(brick); 
        go.transform.parent = this.transform;
        Vector2 loc = new Vector2(xOrigin, yOrigin);
        go.transform.position = loc;
    }

    void Update()
    {
        // move side to side
        //float offset = Mathf.Sin(Time.time * speed) * amplitude / 2;
        //transform.position = new Vector2(offset,transform.position.y);
    }

}
