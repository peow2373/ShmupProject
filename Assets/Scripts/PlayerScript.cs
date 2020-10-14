using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    private float     xPos, yPos;
    public float      speed = .05f;
    public float      leftWall, rightWall, topWall, bottomWall;

    public KeyCode fireKey;

    public GameObject projectile;

    public Sprite[] walkArray;
    public Sprite[] swingArray;
    public Sprite[] chargeArray;
    public Sprite standing;
    public Sprite dead;
    private SpriteRenderer spriteRenderer;
    
    private int currentFrame;
    private int frame;
    private float timer;
    public float frameRate = 1f;

    public static bool flipped;
    public static bool swinging = false;
    private float swingTime;
    private bool swordFire = true;

    private bool swingingSword = false;
    private bool charging = false;

    private BoxCollider2D boxCollider;
    public GameObject sword;
    private CircleCollider2D swordCollider;

    private float timed = 0;
    private bool ignoreInput = false;
    public Animator animator;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        swordCollider = sword.GetComponent<CircleCollider2D>();
    }
    
    // Start is called before the first frame update
    void Start() {
        swordCollider.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        
        // animation
        //timer += Time.deltaTime;

        // player flipped?
        //if (spriteRenderer.flipX) {
        //    flipped = true;
        //} else {
        //    flipped = false;
        //}
        
        SamuraiCharge();

        if (!swingingSword)
        {
            if (!ignoreInput)
            {
                // move left
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    if (xPos > leftWall) {
                        xPos -= speed;
                        animator.SetBool("isAnimated", true);
                        
                        SamuraiCharge();
                        if (charging)
                        {
                            animator.SetBool("isCharged", true);
                            transform.rotation = new Quaternion(0, 180, 0,0);
                            flipped = true;
                        }
                        else
                        {
                            animator.SetBool("isCharged", false);
                            transform.rotation = new Quaternion(0, 180, 0,0);
                            flipped = true;
                        }
                    }
                }
            
                if (Input.GetKeyUp(KeyCode.LeftArrow)) {
                    transform.rotation = new Quaternion(0, 180, 0,0);
                    flipped = true;
                    SamuraiCharge();
                    animator.SetBool("isAnimated", false);
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
                        animator.SetBool("isAnimated", true);
                    
                        SamuraiCharge();
                        if (charging)
                        {
                            animator.SetBool("isCharged", true);
                            transform.rotation = new Quaternion(0, 0, 0,0);
                            flipped = false;
                        }
                        else
                        {
                            animator.SetBool("isCharged", false);
                            transform.rotation = new Quaternion(0, 0, 0,0);
                            flipped = false;
                        }
                    }
                }
            
                if (Input.GetKeyUp(KeyCode.RightArrow)) {
                    transform.rotation = new Quaternion(0, 0, 0,0);
                    flipped = false;
                    SamuraiCharge();
                    animator.SetBool("isAnimated", false);
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
                        animator.SetBool("isAnimated", true);
                    
                        SamuraiCharge();
                        if (charging)
                        {
                            animator.SetBool("isCharged", true);
                        }
                        else
                        {
                            animator.SetBool("isCharged", false);
                        }
                    }
                }
            
                if (Input.GetKeyUp(KeyCode.DownArrow)) {
                    SamuraiCharge();
                    animator.SetBool("isAnimated", false);
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
                        animator.SetBool("isAnimated", true);
                    
                        SamuraiCharge();
                        if (charging)
                        {
                            animator.SetBool("isCharged", true);
                        }
                        else
                        {
                            animator.SetBool("isCharged", false);
                        }
                    }
                }
            
                if (Input.GetKeyUp(KeyCode.UpArrow)) {
                    SamuraiCharge();
                    animator.SetBool("isAnimated", false);
                    if (charging)
                    {
                        spriteRenderer.sprite = swingArray[0];
                    }
                    else
                    {
                        spriteRenderer.sprite = standing;
                    }
                } 
                
                // swing the sword
                if (Input.GetKeyDown(fireKey))
                {
                    animator.SetBool("isAnimated", false);
                    
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
            speed = 0.025f;
            boxCollider.size = new Vector2(1.4f,1.6f);
            boxCollider.offset = new Vector2(0.2f, 0.1f);
        }

        transform.localPosition = new Vector3(xPos, yPos, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            timed = Time.time;
        }
        
        //Charging Combo
        if (Time.time - timed < 0.5f)
        {
            GameManagerScript.chargeCombo = 1f;
        } 
        else if (Time.time - timed > 0.5f && Time.time - timed < 1f)
        {
            GameManagerScript.chargeCombo = 1.5f;
        } 
        else if (Time.time - timed >= 1.5f)
        {
            GameManagerScript.chargeCombo = 2f;
        }

        GameOver();
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
                boxCollider.isTrigger = true;
                swordCollider.enabled = true;
                swordCollider.isTrigger = true;
            }
            if (timeDiff >= standardTime * 2)
            {
                spriteRenderer.sprite = swingArray[2];
            }
            if (timeDiff >= standardTime * 3)
            {
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
            GameManagerScript.chargeCombo = 1f;
        }
    }
    
    private void GameOver()
    {
        if (GameManagerScript.endGame)
        {
            if (transform.position.y > bottomWall - 2.0f)
            {
                spriteRenderer.sprite = dead;
                ignoreInput = true;
                // make player sprite fall
                yPos -= 0.15f;
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}

