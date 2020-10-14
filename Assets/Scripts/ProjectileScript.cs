using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float               speed = 2f;
    public int xDirection, yDirection;
    private Rigidbody2D        rb;
    private BoxCollider2D bc;
    public GameObject          explosion;

    public float minSpeed, maxSpeed, torque;

    public bool flipX;
    private SpriteRenderer spriteRenderer;

    private float time;

    public GameObject projectile;
    private bool temp = true;
    private bool temp1 = true;
    private bool temp2 = true;
    private Vector2 loc;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        flipX = PlayerScript.flipped;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        torque = UnityEngine.Random.Range(-0.8f, -0.5f);
        StartCoroutine("Launch");
    }

    void Update()
    {
        if (temp)
        {
            loc = new Vector2(this.transform.position.x, this.transform.position.y);
        }
    }
    
    private IEnumerator Launch()
    {
        time = Time.time;
        
        if (flipX)
        {
            spriteRenderer.flipX = true;
            xDirection = -1;
        } else {
            spriteRenderer.flipX = false;
            xDirection = 1;
        }
        
        // yield return new WaitForSeconds(1);
        rb.AddForce(transform.right * speed * xDirection);
        rb.AddForce(transform.up * speed * yDirection);
        rb.AddTorque(torque * xDirection);
        yield return null;
    }

    // while player is swinging
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player" || other.gameObject.tag == "player sword")
        {
            if (Time.time - time > 0.5f)
            {
                rb.velocity = new Vector2(rb.velocity.x * -1.1f, rb.velocity.y);
                //rb.angularVelocity *= -1.1f;

                if (temp1)
                {
                    //POINTS: block another sword
                    GameManagerScript.score += 50;
                    temp1 = false;
                }
            }
            return;
        }

        if (other.gameObject.tag == "enemy" || other.gameObject.tag == "enemy sword")
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    // while player isn't swinging
    private void OnCollisionEnter2D(Collision2D other)
    {
        temp = false;

        if (other.gameObject.tag == "enemy" || other.gameObject.tag == "enemy sword")
        {
            //determine which direction the enemy is facing
            Vector2 loc1 = this.gameObject.GetComponent<Transform>().position;
            Vector2 loc2 = other.gameObject.GetComponent<Transform>().position;
            Vector2 direction = loc2 - loc1;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle > 360) angle -= 360;
            if (angle < 0) angle += 360;

            if (Time.time - time > 0.15f)
            {
                //determine which direction the enemy is facing
                if (angle < 90 || angle > 270)
                {
                    if (other.gameObject.GetComponent<EnemyScript>().hasSword == false)
                    {
                        // catch sword
                        Destroy(this.gameObject);
                        other.gameObject.GetComponent<EnemyScript>().hasSword = true;
                        other.gameObject.GetComponent<EnemyScript>().ninjaLife = 1;
                    }
                    else
                    {
                        // make sword ninja lose a life
                        other.gameObject.GetComponent<EnemyScript>().isDying = true;
                        other.gameObject.GetComponent<EnemyScript>().timeStop = true;
                        Destroy(this.gameObject);
                    }
                    
                }
                else
                {
                    other.gameObject.GetComponent<EnemyScript>().isDying = true;
                    other.gameObject.GetComponent<EnemyScript>().timeStop = true;
                    Destroy(this.gameObject);
                }
                
                //POINTS: Hit enemy at start of sword launch
                other.gameObject.GetComponent<EnemyScript>().tooEarly = false;
            }
            else
            {
                //early sword launch
                other.gameObject.GetComponent<EnemyScript>().isDying = true;
                other.gameObject.GetComponent<EnemyScript>().timeStop = true;
                Instantiate(projectile, loc, Quaternion.identity);
                
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Destroy(this.gameObject);
                
                //POINTS: Hit enemy at start of sword launch
                other.gameObject.GetComponent<EnemyScript>().tooEarly = true;
            }
        }
        
        if (other.gameObject.tag == "player")
        {
            if (Time.time - time > 0.5f)
            {
                Destroy(this.gameObject);
                if (temp2)
                {
                    //LIVES LOST: Sword hits player
                    GameManagerScript.lives--;
                    temp2 = false;
                }
            }
            else
            {
                return;
            }
        }
        
        if (other.gameObject.tag == "player sword")
        {
            return;
        }
        
        if (other.gameObject.tag == "wall")
        {
            SpeedCheck();
        }
        
        if (other.gameObject.tag == "projectile")
        {    
            if (Time.time - time < 5.0f)
            {
                //Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        }
        
        if (other.gameObject.tag == "enemy sword")
        {
            if (!other.gameObject.GetComponent<EnemyScript>().hasSword)
            {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
            else
            {
                return;
            }
        }
    }
    
    private void SpeedCheck() {
        
        // Prevent too shallow of an angle
        if (Mathf.Abs(rb.velocity.x) < minSpeed) {
            // shorthand to check for existing direction
            rb.velocity = new Vector2((rb.velocity.x < 0) ? -minSpeed : minSpeed, rb.velocity.y);
        }

        if (Mathf.Abs(rb.velocity.y) < minSpeed) {
            // shorthand to check for existing direction
            rb.velocity = new Vector2(rb.velocity.x, (rb.velocity.y < 0) ? -minSpeed : minSpeed);
        }
    }
}
