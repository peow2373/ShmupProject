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
    private BoxCollider2D bc;
    private CircleCollider2D cc;
    private Vector2 movement;
    public float moveSpeed = 3f;

    public Sprite[] runArray;
    public Sprite[] swordArray;
    public Sprite[] hitArray;
    public Sprite[] swingArray;
    private SpriteRenderer sr;

    public bool hasSword = false;
    private int currentFrame;
    private float timer;
    public float frameRate = 0.1f;
    private float dist;
    public float attackDist;
    private bool attacking = false;
    private float attackTime;
    private bool getTime = true;
    private bool canMove = true;
    private int ninjaLife = 1;
    public Sprite ninjaHit;
    public int spawnRate = 80;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        bc = this.GetComponent<BoxCollider2D>();
        cc = this.GetComponent<CircleCollider2D>();
        player = GameObject.Find("Player");
        
        float delay = Random.Range(2f, 10f);
        float rate = Random.Range(2f, 8f);
        //InvokeRepeating("Fire",delay,rate);
        SpawnType();
    }

    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle > 360) angle -= 360;
        if (angle < 0) angle += 360;
        if (angle >= 90 && angle <= 270)
        {
            transform.rotation = new Quaternion(0, 180, 0,0);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0,0);
        }
        rb.rotation = 0;
        direction.Normalize();
        movement = direction;
        
        timer += Time.deltaTime;
        dist = Vector3.Distance(player.transform.position, transform.position);

        // animate the running ninja
        if (dist < attackDist)
        {
            if (!attacking)
            {
                if (getTime)
                {
                    attackTime = Time.time;
                    attacking = true;
                    getTime = false;
                }
            }
        }

        if (canMove && !attacking)
        {
            //bc.size = new Vector2(1.2f,bc.size.y);
            //bc.offset = new Vector2(0f,bc.offset.y);
            cc.radius = 0.9f;
            cc.offset = new Vector2(0f,0f);
            
            if (!hasSword)
            {
                // animate running ninja
                if (timer >= frameRate)
                {
                    timer -= frameRate;
                    currentFrame = (currentFrame + 1) % runArray.Length;
                    sr.sprite = runArray[currentFrame];
                }
            }
            else
            {
                // animate running sword ninja
                if (timer >= frameRate)
                {
                    timer -= frameRate;
                    currentFrame = (currentFrame + 1) % swordArray.Length;
                    sr.sprite = swordArray[currentFrame];
                }
            }
        }
        
        if (!hasSword)
        {
            // animate attacking ninja
            Attack(hitArray, 1.6f, 0.05f);
        }
        else
        {
            // animate attacking sword ninja
            Attack(swingArray, 2.5f, 0.45f);
        }
    }

    private void SpawnType()
    {
        int i = Random.Range(0, 100);
        if (i > spawnRate)
        {
            hasSword = true;
        }
        else
        {
            hasSword = false;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            moveCharacter(movement);  
        }
    }

    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player" || other.gameObject.tag == "player sword")
        {
            if (!hasSword)
            {
                Destroy(this.gameObject);
                Debug.Log("enemy slain!");
            }
            else
            {
                if (ninjaLife == 1)
                {
                    sr.sprite = ninjaHit;
                    ninjaLife = 0;
                    hasSword = false;
                } 
                else if (ninjaLife == 0)
                {
                    Destroy(this.gameObject);
                    Debug.Log("enemy slain!");
                }
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "player")
        {
            Destroy(this.gameObject);
            Debug.Log("ouch!");
        }
        
        if (other.gameObject.tag == "player sword" || other.gameObject.tag == "enemy")
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    private void Attack(Sprite[] attackArray, float colliderSize, float colliderOffset)
    {
        if (attacking)
        {
            float timeDiff = Time.time - attackTime;
            float standardTime = frameRate * 1f;
            
            canMove = false;
            sr.sprite = attackArray[0];

            if (timeDiff >= standardTime)
            {
                sr.sprite = attackArray[1];
            }
            if (timeDiff >= standardTime * 1.5)
            {
                //bc.size = new Vector2(colliderSize,bc.size.y);
                //bc.offset = new Vector2(colliderOffset,bc.offset.y);
            }
            if (timeDiff >= standardTime * 2)
            {
                sr.sprite = attackArray[2];
                canMove = true;
            }
            if (timeDiff >= standardTime * 4)
            {
                getTime = true;
                attacking = false;
            }
        }
    }
}
