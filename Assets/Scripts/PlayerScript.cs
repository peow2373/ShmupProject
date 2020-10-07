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
    public Sprite standing;
    private SpriteRenderer spriteRenderer;
    
    private int currentFrame;
    private int frame;
    private float timer;
    public float frameRate = 0.1f;

    public static bool flipped;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    
    // Start is called before the first frame update
    void Start() {
        healthBar.fillAmount = health;
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

        // move left
        if (Input.GetKey(KeyCode.LeftArrow)) {
            if (xPos > leftWall) {
                xPos -= speed;
                
                if (timer >= frameRate)
                {
                    timer -= frameRate;
                    currentFrame = (currentFrame + 1) % walkArray.Length;
                    spriteRenderer.flipX = true;
                    spriteRenderer.sprite = walkArray[currentFrame];
                }
            }
        }
        
        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            spriteRenderer.flipX = true;
            spriteRenderer.sprite = standing;
        }

        // move right
        if (Input.GetKey(KeyCode.RightArrow)) {
            if (xPos < rightWall) {
                xPos += speed;
                
                if (timer >= frameRate)
                {
                    timer -= frameRate;
                    currentFrame = (currentFrame + 1) % walkArray.Length;
                    spriteRenderer.flipX = false;
                    spriteRenderer.sprite = walkArray[currentFrame];
                }
            }
        }
        
        if (Input.GetKeyUp(KeyCode.RightArrow)) {
            spriteRenderer.flipX = false;
            spriteRenderer.sprite = standing;
        }
        
        // move down
        if (Input.GetKey(KeyCode.DownArrow)) {
            if (yPos > bottomWall) {
                yPos -= speed;
                
                if (timer >= frameRate)
                {
                    timer -= frameRate;
                    currentFrame = (currentFrame + 1) % walkArray.Length;
                    spriteRenderer.sprite = walkArray[currentFrame];
                }
            }
        }
        
        if (Input.GetKeyUp(KeyCode.DownArrow)) {
            spriteRenderer.sprite = standing;
        }

        // move up
        if (Input.GetKey(KeyCode.UpArrow)) {
            if (yPos < topWall) {
                yPos += speed;
                
                if (timer >= frameRate)
                {
                    timer -= frameRate;
                    currentFrame = (currentFrame + 1) % walkArray.Length;
                    spriteRenderer.sprite = walkArray[currentFrame];
                }
            }
        }
        
        if (Input.GetKeyUp(KeyCode.UpArrow)) {
            spriteRenderer.sprite = standing;
        }

        // swing the sword
        if (Input.GetKeyDown(fireKey))
        {
            Instantiate(projectile, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
        }

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
}

