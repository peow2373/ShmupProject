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
    private bool temp = true;

    public GameObject projectile;
    public Vector2 loc;

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
                Debug.Log("blocked!");
            }
            return;
        }

        if (other.gameObject.tag == "wall")
        {    
            //Destroy(this.gameObject);
        }
    }

    // while player isn't swinging
    private void OnCollisionEnter2D(Collision2D other)
    {
        temp = false;
        
        if (other.gameObject.tag == "enemy")
        {
            this.GetComponent<Renderer>().enabled = false;

            // award points
            if (Time.time - time < 0.025f)
            {
                Debug.Log("enemy slain!");
                Destroy(this.gameObject);
                Destroy(other.gameObject);
                Instantiate(projectile, loc, Quaternion.identity);
            }
            else if (Time.time - time >= 0.025f && Time.time - time < 0.15f)
            {
                Debug.Log("enemy slain!");
                Destroy(this.gameObject);
                Destroy(other.gameObject);
                Instantiate(projectile, loc, Quaternion.identity);
            }
            else
            {
                //bc.sharedMaterial = bouncy;
                Debug.Log("enemy slain!");
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
        
        if (other.gameObject.tag == "player")
        {
            if (Time.time - time > 0.5f)
            {
                Destroy(this.gameObject);
                Debug.Log("ouch!");
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
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
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
