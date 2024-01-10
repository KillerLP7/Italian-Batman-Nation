using System;
using UnityEngine;
using Random = System.Random;

public class Enemy : MonoBehaviour
{
    public GameObject[] enemys = new GameObject[0];

    Rigidbody2D rb;
    private Vector3 targetPos;
    private float counter;
    private int health;


    private bool askForHelp = false;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (enemys[0])
        {
            health = 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 nv = new Vector2();
        
        if (targetPos.y > transform.position.y)
        {
            nv += new Vector2(0, 1);
        }

        if (targetPos.y < transform.position.y)
        {
            nv += new Vector2(0, -1);
        }
        
        if (Math.Abs(targetPos.y - transform.position.y) < 0.1f)
        {
            if (targetPos.x > transform.position.x)
            {
                nv += new Vector2(1, 0);
            }
            
            if (targetPos.x < transform.position.x)
            {
                nv += new Vector2(-1, 0);
            }

            
            
            if (Math.Abs(targetPos.x - transform.position.x) < 0.1f)
            {
                askForHelp = true;
                targetPos = GameManager.Instance.Chase(askForHelp);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameObject newEnemy = Instantiate(enemys[0], new Vector3(4,0,0), Quaternion.identity);
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        
        if (counter > 5)
        {
            Ask();
            counter = 0;
        }

        counter += Time.deltaTime;
        rb.velocity = nv.normalized;
    }

    void Ask()
    {
        targetPos = GameManager.Instance.Chase(askForHelp);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Sprite attack
            //Player Health Bar drops
            print("GAME OVER");
        }

        if (collision.gameObject.CompareTag("Attack"))
        {
            print("health");
            health--;
        }
        
    }
}
