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
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flipX = PlayerScript.flipped;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        torque = UnityEngine.Random.Range(0.0f, 0.5f);
        StartCoroutine("Launch");
    }

    void Update()
    {
        
    }
    
    private IEnumerator Launch() {
        
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

    // enemy projectile
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            return;
        }

        if (other.gameObject.tag == "wall")
        {    
            //Destroy(this.gameObject);
        }
    }

    // player projectile
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            // award points
            //Instantiate(explosion, this.transform.position, quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        
        if (other.gameObject.tag == "wall")
        {
            SpeedCheck();
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
        
        Debug.Log(rb.velocity);
    }
}
