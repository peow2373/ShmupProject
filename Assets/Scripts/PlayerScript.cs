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

    public float walkSpeed;
    public float chargeSpeed;
    public float fallSpeed;

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
        
        // IMPORTANT!!!!!!!!!!! Please Read
        
        // I needed to tweak the values of some of the speed variables in order ofr the game to work on WebGL
        // In Unity, I normally use the following:
        // walkSpeed = 0.025;
        // chargeSpeed = 0.02;
        // fallSpeed = 0.015;
        
        // In WebGL, I use the values:
        // walkSpeed = 0.125;
        // chargeSpeed = 0.1;
        // fallSpeed = 0.11;
        
        SamuraiCharge();

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow))
        {
            animator.gameObject.GetComponent<Animator>().enabled = true;
            animator.SetBool("isAnimated", true);
        }
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow) &&
            !Input.GetKey(KeyCode.DownArrow))
        {
            if (animator.GetBool("isSwinging") || animator.GetBool("isCharged"))
            {
                spriteRenderer.sprite = swingArray[0];
                //animator.SetBool("isAnimated", true);
            }
            else
            {
                spriteRenderer.sprite = standing;
                animator.gameObject.GetComponent<Animator>().enabled = false;
            }
        }

        if (!swingingSword)
        {
            if (!ignoreInput)
            {
                // move left
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    if (xPos > leftWall) {
                        xPos -= speed;
                        
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
                }

                // move right
                if (Input.GetKey(KeyCode.RightArrow)) {
                    if (xPos < rightWall) {
                        xPos += speed;

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
                }
            
                // move down
                if (Input.GetKey(KeyCode.DownArrow)) {
                    if (yPos > bottomWall) {
                        yPos -= speed;

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
                }

                // move up
                if (Input.GetKey(KeyCode.UpArrow)) {
                    if (yPos < topWall) {
                        yPos += speed;

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
                } 
                
                // swing the sword
                if (Input.GetKeyDown(fireKey))
                {
                    spriteRenderer.sprite = swingArray[0];
                    //animator.SetBool("isCharged", true);
                    animator.SetBool("isSwinging", true);
                    
                    if (!Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow) &&
                        !Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        //animator.gameObject.GetComponent<Animator>().enabled = false;
                        animator.SetBool("isAnimated", false);
                        animator.SetBool("isCharged", false);
                        spriteRenderer.sprite = swingArray[0];
                        swordFire = true;
                    }
                }
                
                if (Input.GetKeyUp(fireKey))
                {
                    swingingSword = true;
                    animator.SetBool("isCharged", false);
                    animator.SetBool("isSwinging", false);

                    if (!Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow) &&
                        !Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        animator.gameObject.GetComponent<Animator>().enabled = false;
                        swinging = true;
                        swingTime = Time.time;
                    }
                }
            }
        }

        if (animator.GetBool("isAnimated") == false)
        {
            if (charging)
            {
                animator.SetBool("isSwinging", true);
            }
            else
            {
                animator.SetBool("isSwinging", false);
            }
        }

        SwingSword();

        if (charging)
        {
            speed = chargeSpeed;
            boxCollider.size = new Vector2(1.8f,1.65f);
            boxCollider.offset = new Vector2(0.1f, 0.1f);
        }
        else
        {
            speed = walkSpeed;
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
                
                animator.gameObject.GetComponent<Animator>().enabled = true;
                animator.SetBool("isCharged", false);
                animator.SetBool("isSwinging", false);
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
                yPos -= fallSpeed;
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}

