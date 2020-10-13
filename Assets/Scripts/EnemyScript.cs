using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyScript : MonoBehaviour
{
    public GameObject enemyProjectile;
    private GameObject player;
    private Rigidbody2D rb;
    private Vector2 movement;
    public float moveSpeed = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        
        float delay = Random.Range(2f, 10f);
        float rate = Random.Range(2f, 8f);
        //InvokeRepeating("Fire",delay,rate);
    }

    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle > 360) angle -= 360;
        if (angle < 0) angle += 360;
        rb.rotation = angle;
        direction.Normalize();
        movement = direction;
    }

    private void Fire()
    {
        int i = Random.Range(0, 100);
        if (i > 80)
        {
            //Instantiate(enemyProjectile, new Vector2(transform.position.x, transform.position.y), quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        moveCharacter(movement);
    }

    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player")
        {
            Destroy(this.gameObject);
            Debug.Log("enemy slain!");
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "player")
        {
            Destroy(this.gameObject);
            Debug.Log("ouch!");
        }
    }
}
