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
    public GameObject          explosion;

    public float minSpeed, maxSpeed, torque;

    public bool flipX;
    private SpriteRenderer spriteRenderer;

    private float time;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flipX = PlayerScript.flipped;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        torque = UnityEngine.Random.Range(0.0f, 0.5f);
        if (!flipX) transform.position = new Vector2(transform.position.x + 0.823f, transform.position.y - 0.87f);
        if (flipX) transform.position = new Vector2(transform.position.x - 0.823f, transform.position.y - 0.87f);
        StartCoroutine("Launch");
    }

    void Update()
    {
        
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
        if (other.gameObject.tag == "player")
        {
            if (Time.time - time > 15.0f)
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
        if (other.gameObject.tag == "enemy")
        {
            // award points
            //Instantiate(explosion, this.transform.position, quaternion.identity);
            Debug.Log("enemy slain!");
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        
        if (other.gameObject.tag == "player")
        {
            Destroy(this.gameObject);
            Debug.Log("ouch!");
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
