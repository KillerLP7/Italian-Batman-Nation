using System;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject[] enemys;
    [SerializeField] private GameObject enemyAttack;

    Rigidbody2D rb;
    private Vector3 targetPos;
    private float counter;
    private int health;
    private bool canAttack;
    private bool enemyLooksRight;
    private Vector3 attackArea;

    private bool kick;
    //public static int activeEnemies;
    
    private bool askForHelp = false;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        for (int i = 0; i < enemys.Length; i++)
        {
            //print(i);
            enemys[i] = GameManager.Instance.EnemyTypes();
        }
        if (enemys[0])
        {
            health = 1;
        }
        if (enemys[1])
        {
            health = 1;
        }
        if (enemys[2])
        {
            health = 1;
        }
        if (enemys[3])
        {
            health = 1;
        }

        GameManager.Instance.ActiveEnemiesAdd();
    }

    // Update is called once per frame
    void Update()
    {
        //rint($"Execute Kick: {kick}");
        print($"Can Attack?: {canAttack}");
        ExecuteAttack();
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
        
        if (counter > 5)
        {
            Ask();
            counter = 0;
        }
        counter += Time.deltaTime;
        rb.velocity = nv.normalized;
        if (!canAttack)
        {
            if (counter > 3)
            {
                canAttack = true;
                counter = 0;
            }
            counter += Time.deltaTime;
        }
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
            print("We got him!");
            GameManager.Instance.PlayerGotHit();
        }

        if (collision.gameObject.CompareTag("Attack"))
        {
            
            
            //print(health);
            health--;
            if (health == 0)
            {
                GameManager.Instance.ActiveEnemiesRemove();
                Destroy(gameObject);
            }
            
        }
        
    }
    private void ExecuteAttack()
    {
        print("Tried to Attack!" + kick);
        
        
        if (canAttack && kick)
        {
            if (enemyLooksRight)
            {
                attackArea = transform.position;
                attackArea.x += 1f;
                attackArea.y += -0.5f;
                Instantiate(enemyAttack, attackArea, quaternion.identity);
                canAttack = false;
                kick = false;
                return;
            }

            attackArea = transform.position;
            attackArea.x += -1f;
            attackArea.y += -0.5f;
            Instantiate(enemyAttack, attackArea, quaternion.identity);
            canAttack = false;
            kick = false;
            return;
        }
        
        
        
        if (canAttack && !kick)
        {
            if (enemyLooksRight)
            {
                attackArea = transform.position;
                attackArea.x += 1f;
                attackArea.y += 0.5f;
                Instantiate(enemyAttack, attackArea, quaternion.identity);
                canAttack = false;
                kick = true;
                return;
            }

            attackArea = transform.position;
            attackArea.x += -1f;
            attackArea.y += 0.5f;
            Instantiate(enemyAttack, attackArea, quaternion.identity);
            canAttack = false;
            kick = true;
        }
        
    }
}
