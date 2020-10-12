﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    private float     xPos, yPos;
    public float      speed = .05f;
    public float      leftWall, rightWall, topWall, bottomWall;
    public float health = 1f;

    public KeyCode fireKey;

    public GameObject projectile;
    public Image healthBar;

    public Sprite[] walkArray;
    public Sprite[] swingArray;
    public Sprite[] chargeArray;
    public Sprite standing;
    private SpriteRenderer spriteRenderer;
    
    private int currentFrame;
    private int frame;
    private float timer;
    public float frameRate = 0.1f;

    public static bool flipped;
    public static bool swinging = false;
    private float swingTime;
    private bool swordFire = true;

    private bool swingingSword = false;
    private bool charging = false;

    public BoxCollider2D boxCollider;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    
    // Start is called before the first frame update
    void Start() {
        //healthBar.fillAmount = health;
        healthBar.fillAmount = 0;
    }

    // Update is called once per frame
    void Update() {
        
        // animation
        timer += Time.deltaTime;

        // player flipped?
        if (spriteRenderer.flipX) {
            flipped = true;
        } else {
            flipped = false;
        }
        
        SamuraiCharge();

        if (!swingingSword)
        {
            // move left
            if (Input.GetKey(KeyCode.LeftArrow)) {
                if (xPos > leftWall) {
                    xPos -= speed;
                    
                    if (timer >= frameRate)
                    {
                        timer -= frameRate;
                        SamuraiCharge();
                        if (charging)
                        {
                            currentFrame = (currentFrame + 1) % chargeArray.Length;
                            spriteRenderer.flipX = true;
                            spriteRenderer.sprite = chargeArray[currentFrame];
                        }
                        else
                        {
                            currentFrame = (currentFrame + 1) % walkArray.Length;
                            spriteRenderer.flipX = true;
                            spriteRenderer.sprite = walkArray[currentFrame];
                        }
                    }
                }
            }
            
            if (Input.GetKeyUp(KeyCode.LeftArrow)) {
                spriteRenderer.flipX = true;
                SamuraiCharge();
                if (charging)
                {
                    spriteRenderer.sprite = swingArray[0];
                }
                else
                {
                    spriteRenderer.sprite = standing;
                }
            }

            // move right
            if (Input.GetKey(KeyCode.RightArrow)) {
                if (xPos < rightWall) {
                    xPos += speed;
                    
                    if (timer >= frameRate)
                    {
                        timer -= frameRate;
                        SamuraiCharge();
                        if (charging)
                        {
                            currentFrame = (currentFrame + 1) % chargeArray.Length;
                            spriteRenderer.flipX = false;
                            spriteRenderer.sprite = chargeArray[currentFrame];
                        }
                        else
                        {
                            currentFrame = (currentFrame + 1) % walkArray.Length;
                            spriteRenderer.flipX = false;
                            spriteRenderer.sprite = walkArray[currentFrame];
                        }
                    }
                }
            }
            
            if (Input.GetKeyUp(KeyCode.RightArrow)) {
                spriteRenderer.flipX = false;
                SamuraiCharge();
                if (charging)
                {
                    spriteRenderer.sprite = swingArray[0];
                }
                else
                {
                    spriteRenderer.sprite = standing;
                }
            }
            
            // move down
            if (Input.GetKey(KeyCode.DownArrow)) {
                if (yPos > bottomWall) {
                    yPos -= speed;
                    
                    if (timer >= frameRate)
                    {
                        timer -= frameRate;
                        SamuraiCharge();
                        if (charging)
                        {
                            currentFrame = (currentFrame + 1) % chargeArray.Length;
                            spriteRenderer.sprite = chargeArray[currentFrame];
                        }
                        else
                        {
                            currentFrame = (currentFrame + 1) % walkArray.Length;
                            spriteRenderer.sprite = walkArray[currentFrame];
                        }
                    }
                }
            }
            
            if (Input.GetKeyUp(KeyCode.DownArrow)) {
                SamuraiCharge();
                if (charging)
                {
                    spriteRenderer.sprite = swingArray[0];
                }
                else
                {
                    spriteRenderer.sprite = standing;
                }
            }

            // move up
            if (Input.GetKey(KeyCode.UpArrow)) {
                if (yPos < topWall) {
                    yPos += speed;
                    
                    if (timer >= frameRate)
                    {
                        timer -= frameRate;
                        SamuraiCharge();
                        if (charging)
                        {
                            currentFrame = (currentFrame + 1) % chargeArray.Length;
                            spriteRenderer.sprite = chargeArray[currentFrame];
                        }
                        else
                        {
                            currentFrame = (currentFrame + 1) % walkArray.Length;
                            spriteRenderer.sprite = walkArray[currentFrame];
                        }
                    }
                }
            }
            
            if (Input.GetKeyUp(KeyCode.UpArrow)) {
                SamuraiCharge();
                if (charging)
                {
                    spriteRenderer.sprite = swingArray[0];
                }
                else
                {
                    spriteRenderer.sprite = standing;
                }
            }
        }

        // swing the sword
        if (Input.GetKeyDown(fireKey))
        {
            if (!Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow) &&
                !Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.UpArrow))
            {
                spriteRenderer.sprite = swingArray[0];
                swordFire = true;
            }
        }
        
        if (Input.GetKeyUp(fireKey))
        {
            swingingSword = true;
            
            if (!Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow) &&
                !Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.UpArrow))
            {
                swinging = true;
                swingTime = Time.time;
            }
        }

        SwingSword();

        transform.localPosition = new Vector3(xPos, yPos, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.gameObject.tag == "enemy projectile")
        {
            Destroy(other.gameObject);
            health -= 0.1f;
            healthBar.fillAmount = health;
        }
    }

    private void SwingSword()
    {
        if (swinging)
        {
            float timeDiff = Time.time - swingTime;
            float standardTime = frameRate * 0.4f;
            boxCollider.isTrigger = true;

            if (timeDiff >= standardTime)
            {
                spriteRenderer.sprite = swingArray[1];
                boxCollider.size = new Vector2(boxCollider.size.x,2.6f);
                boxCollider.offset = new Vector2(boxCollider.offset.x,0.4f);
            }
            if (timeDiff >= standardTime * 2)
            {
                spriteRenderer.sprite = swingArray[2];
                boxCollider.size = new Vector2(3.0f,2.6f);
                boxCollider.offset = new Vector2(0.5f,0.4f);
            }
            if (timeDiff >= standardTime * 3)
            {
                spriteRenderer.sprite = swingArray[3];
                boxCollider.size = new Vector2(3.4f,2.3f);
                boxCollider.offset = new Vector2(0.6f,0.3f);
            }
            if (timeDiff >= standardTime * 4)
            {
                boxCollider.size = new Vector2(3.6f,2.4f);
                boxCollider.offset = new Vector2(0.6f,0.0f);
                spriteRenderer.sprite = swingArray[4];
            }
            if (timeDiff >= standardTime * 4.75)
            {
                if (swordFire)
                {
                    // launch sword
                    //Instantiate(projectile, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                    swordFire = false;
                }
            }
            if (timeDiff >= standardTime * 5)
            {
                spriteRenderer.sprite = swingArray[5];
            }
            if (timeDiff >= standardTime * 6)
            {
                swingingSword = false;
            }
            if (timeDiff >= standardTime * 7)
            {
                spriteRenderer.sprite = standing;
                boxCollider.isTrigger = false;
                boxCollider.size = new Vector2(2.1f,1.65f);
                boxCollider.offset = new Vector2(0.2f, 0.1f);
                swinging = false;
            }
        }
    }

    private void SamuraiCharge()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            charging = true;
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            charging = false;
        }
    }
}

