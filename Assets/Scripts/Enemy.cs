using System;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject[] enemys;
    [SerializeField] private GameObject enemyAttack;
    [SerializeField] private bool boss;
    Rigidbody2D rb;
    private Vector3 targetPos;
    private float counter;
    private float bossCooldown;
    private int health;
    private bool canAttack;
    private bool enemyLooksRight;
    private Vector3 attackArea;
    private bool bossEnter;

    private bool kick;
    //public static int activeEnemies;
    
    private bool askForHelp;
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

        if (boss)
        {
            health = 30;
            bossEnter = true;
        }

        askForHelp = true;
        Ask();
        GameManager.Instance.ActiveEnemiesAdd();
    }

    // Update is called once per frame
    void Update()
    {
        if (!boss)
        {
            //print($"Ask for help: {askForHelp}");
            //print($"Can Attack?: {transform.position}");
            ExecuteAttack();
            Vector2 nv = new Vector2();
            if (askForHelp)
            {
                Ask();
                askForHelp = false;
            }
            if (counter > 3)
            {
                askForHelp = true;
                //targetPos = GameManager.Instance.Chase(askForHelp);
                Ask();
                counter = 0;
            }
            counter += Time.deltaTime;
            if (targetPos.y > transform.position.y)
            {
                nv += new Vector2(0, 1);
            }
            else if (targetPos.y < transform.position.y)
            {
                nv += new Vector2(0, -1);
            }
            if (Math.Abs(targetPos.y - transform.position.y) < 0.1f)
            {
                if (targetPos.x - 1f > transform.position.x)
                {
                    enemyLooksRight = true;
                    nv += new Vector2(1, 0);
                }
            
                if (targetPos.x + 1f < transform.position.x)
                {
                    enemyLooksRight = false;
                    nv += new Vector2(-1, 0);
                }
                if (Math.Abs(targetPos.x + 1 - transform.position.x) < 0.1f)
                {
                    //nv += new Vector2(0, 0);
                }
            
                if (Math.Abs(targetPos.x - 1 + transform.position.x) < 0.1f)
                {
                    //nv += new Vector2(0, 0);
                }
            
            }
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

        if (boss)
        {
            if (health == 20 || health == 10)
            {
                if (bossEnter && transform.position.x < 9f)
                {
                    bossEnter = false;
                }
            }
            if (!bossEnter && bossCooldown > 10)
            {
                bossEnter = true;
                bossCooldown = 0;
            }
            bossCooldown += Time.deltaTime;
            if (bossEnter && transform.position.x > 8.7f)
            {
                rb.velocity = new Vector2(-1, 0);
            }
            else if (!bossEnter && transform.position.x < 12f)
            {
                rb.velocity = new Vector2(1, 0);
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
            }
            print($"Boss Health: {health}");
            
        }
    }

    void Ask()
    {
        targetPos = GameManager.Instance.Chase(askForHelp);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!boss)
        {
            if (collision.gameObject.CompareTag("Attack"))
            {
                health--;
                if (health == 0)
                {
                    GameManager.Instance.ActiveEnemiesRemove();
                    Destroy(gameObject);
                }
            
            }
        }

        if (boss)
        {
            if (collision.gameObject.CompareTag("Attack"))
            {
                print("Did I got him?");
                health--;
                GameManager.Instance.BossGotHit();
                if (health == 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        
    }
    private void ExecuteAttack()
    {
        //print("Tried to Attack!" + kick);
        
        
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
