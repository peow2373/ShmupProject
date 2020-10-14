using System;
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

    private BoxCollider2D boxCollider;
    public GameObject sword;
    private CircleCollider2D swordCollider;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        swordCollider = sword.GetComponent<CircleCollider2D>();
    }
    
    // Start is called before the first frame update
    void Start() {
        //healthBar.fillAmount = health;
        healthBar.fillAmount = 0;
        swordCollider.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        
        // animation
        timer += Time.deltaTime;

        // player flipped?
        //if (spriteRenderer.flipX) {
        //    flipped = true;
        //} else {
        //    flipped = false;
        //}
        
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
                            transform.rotation = new Quaternion(0, 180, 0,0);
                            flipped = true;
                            spriteRenderer.sprite = chargeArray[currentFrame];
                        }
                        else
                        {
                            currentFrame = (currentFrame + 1) % walkArray.Length;
                            transform.rotation = new Quaternion(0, 180, 0,0);
                            flipped = true;
                            spriteRenderer.sprite = walkArray[currentFrame];
                        }
                    }
                }
            }
            
            if (Input.GetKeyUp(KeyCode.LeftArrow)) {
                transform.rotation = new Quaternion(0, 180, 0,0);
                flipped = true;
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
                            transform.rotation = new Quaternion(0, 0, 0,0);
                            flipped = false;
                            spriteRenderer.sprite = chargeArray[currentFrame];
                        }
                        else
                        {
                            currentFrame = (currentFrame + 1) % walkArray.Length;
                            transform.rotation = new Quaternion(0, 0, 0,0);
                            flipped = false;
                            spriteRenderer.sprite = walkArray[currentFrame];
                        }
                    }
                }
            }
            
            if (Input.GetKeyUp(KeyCode.RightArrow)) {
                transform.rotation = new Quaternion(0, 0, 0,0);
                flipped = false;
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

        if (charging)
        {
            speed = 0.02f;
            boxCollider.size = new Vector2(1.8f,1.65f);
            boxCollider.offset = new Vector2(0.1f, 0.1f);
        }
        else
        {
            speed = 0.03f;
            boxCollider.size = new Vector2(1.4f,1.6f);
            boxCollider.offset = new Vector2(0.2f, 0.1f);
        }

        transform.localPosition = new Vector3(xPos, yPos, 0);
    }

    private void SwingSword()
    {
        if (swinging)
        {
            float timeDiff = Time.time - swingTime;
            float standardTime = frameRate * 0.4f;

            if (timeDiff >= standardTime)
            {
                spriteRenderer.sprite = swingArray[1];
            }
            if (timeDiff >= standardTime * 2)
            {
                spriteRenderer.sprite = swingArray[2];
            }
            if (timeDiff >= standardTime * 3)
            {
                boxCollider.isTrigger = true;
                swordCollider.enabled = true;
                swordCollider.isTrigger = true;
                spriteRenderer.sprite = swingArray[3];
            }
            if (timeDiff >= standardTime * 4)
            {
                spriteRenderer.sprite = swingArray[4];
            }
            if (timeDiff >= standardTime * 4.75)
            {
                if (swordFire)
                {
                    // launch sword
                    if (flipped) Instantiate(projectile, new Vector2(transform.position.x - 0.823f, transform.position.y + 0.5f - 0.87f), Quaternion.identity);
                    if (!flipped) Instantiate(projectile, new Vector2(transform.position.x + 0.823f, transform.position.y + 0.5f - 0.87f), Quaternion.identity);
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
                swordCollider.enabled = false;
                swordCollider.isTrigger = false;
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

