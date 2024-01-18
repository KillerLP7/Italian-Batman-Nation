using System;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private Animator anime;
    private SpriteRenderer sr;
    Rigidbody2D rb;
    [SerializeField] private GameObject[] enemys;
    [SerializeField] private GameObject enemyAttack;
    [SerializeField] private GameObject bossAttack;
    [SerializeField] private bool firstEnemy;
    [SerializeField] private GameObject bomb;
    [SerializeField] private bool secondEnemy;
    [SerializeField] private bool thirdEnemy;
    [SerializeField] private bool fourthEnemy;
    [SerializeField] private bool catBombEnemy;
    [SerializeField] private bool boss;
    
    [SerializeField] private float enemySpeed;
    private Vector3 targetPos;
    private float counter;
    private float bossCooldown;
    private int health;
    private bool canAttack;
    private bool enemyLooksRight;
    private Vector3 attackArea;
    private Vector3 explosionArea;
    private bool bossEnter;
    private float bombCooldown;
    private float rnd;
    private bool bossCanDie;

    private bool kick;
    //public static int activeEnemies;
    
    private bool askForHelp;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
        for (int i = 0; i < enemys.Length; i++)
        {
            //print(i);
            enemys[i] = GameManager.Instance.EnemyTypes()[i];
        }
        if (firstEnemy)
        {
            health = 1;
        }
        if (secondEnemy)
        {
            health = 2;
        }
        if (thirdEnemy)
        {
            health = 4;
        }
        if (fourthEnemy)
        {
            health = 8;
        }

        if (catBombEnemy)
        {
            health = 1;
        }

        if (boss)
        {
            health = 30;
            bossEnter = true;
            bossCanDie = false;
        }

        canAttack = true;
        askForHelp = true;
        enemyLooksRight = true;
        Ask();
        GameManager.Instance.ActiveEnemiesAdd();
    }

    // Update is called once per frame
    void Update()
    {
        print($"Die Werte: CanAttack: {canAttack} EnemyLooksRight {enemyLooksRight}");
        if (!boss)
        {
            //print($"Ask for help: {askForHelp}");
            //print($"Can Attack?: {transform.position}");
            if (!catBombEnemy)
            {
                ExecuteAttack();
            }
            Vector2 nv = new Vector2();
            if (askForHelp)
            {
                Ask();
                askForHelp = false;
            }
            if (counter > 1.5f)
            {
                askForHelp = true;
                //targetPos = GameManager.Instance.Chase(askForHelp);
                Ask();
                counter = 0;
            }
            counter += Time.deltaTime;
            if (targetPos.y > transform.position.y)
            {
                nv += new Vector2(0, 0f);
                anime.SetBool("EnemyWalking", false);
            }
            else if (targetPos.y < transform.position.y)
            {
                nv += new Vector2(0, 0f);
                anime.SetBool("EnemyWalking", false);
            }
            if (targetPos.y < transform.position.y || targetPos.y > transform.position.y)
            {
                if (targetPos.x - 1f > transform.position.x)
                {
                    enemyLooksRight = true;
                    sr.flipX = true;
                    anime.SetBool("EnemyWalking", true);
                    nv += new Vector2(1, 0);
                }
            
                if (targetPos.x + 1f < transform.position.x)
                {
                    enemyLooksRight = false;
                    sr.flipX = false;
                    anime.SetBool("EnemyWalking", true);
                    nv += new Vector2(-1, 0);
                }
                if (targetPos.y - 1f > transform.position.y)
                {
                    nv += new Vector2(0, 1);
                }
            
                if (targetPos.y - 1f < transform.position.y || Math.Abs(targetPos.y - 1f - transform.position.y) < 0.1f)
                {
                    nv += new Vector2(0, -1);
                }
                if (Math.Abs(targetPos.x + 1.5f - transform.position.x) < 0.1f)
                {
                    //anime.SetBool("EnemyWalking", false);
                }
            
                if (Math.Abs(targetPos.x - 1.5f + transform.position.x) < 0.1f)
                {
                    //anime.SetBool("EnemyWalking", false);
                }
                nv += new Vector2(0, 0);
            
            }

            
            
            rb.velocity = nv.normalized * enemySpeed;
            if (!catBombEnemy)
            {
                if (!canAttack)
                {
                    if (counter > 1.5f)
                    {
                        canAttack = true;
                        counter = 0;
                    }
                    counter += Time.deltaTime;
                }
            }
        }
        if (boss)
        {
            bossCanDie = GameManager.Instance.GetBossCanDie();
            health = GameManager.Instance.GetBossHP();
            GameManager.Instance.BossPhase();
            if (!canAttack)
            {
                if (counter > 0.1)
                {
                    canAttack = true;
                    counter = 0;
                }
                counter += Time.deltaTime;
            }
            ExecuteAttack();
            if (health == 110)
            {
                anime.SetBool("NextBossPhase", true);
            }
            if (health == 200 || health == 100 || health == 0)
            {
                anime.SetBool("NextBossPhase", false);
                //TODO: Execute only once
                if (bossEnter && transform.position.x < 9f)
                {
                    bossEnter = false;
                }
            }
            if (!bossEnter && bossCooldown > 30)
            {
                bossEnter = true;
                bossCooldown = 0;
            }
            bossCooldown += Time.deltaTime;
            if (bossEnter && transform.position.x > 8.7f)
            {
                rb.velocity = new Vector2(-1, 0);
            }
            else if (!bossEnter && transform.position.x < 15f)
            {
                rb.velocity = new Vector2(1, 0);
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
            }
            //print($"Boss Health: {health}");
            
        }

        if (catBombEnemy)
        {
            print("Cat rnd:" + rnd);
            if (health == 0)
            {
                explosionArea = transform.position;
                explosionArea.x += 0;
                explosionArea.y += -0.5f;
                Instantiate(bomb, explosionArea, Quaternion.identity);
                Destroy(gameObject);
            }
            rnd = Random.Range(30, 60);
            print("Cat rnd:" + rnd);
            if (bombCooldown > rnd)
            {
                health--;
                bombCooldown = 0;
            }
            bombCooldown += Time.deltaTime;
        }
        
    }

    void Ask()
    {
        targetPos = GameManager.Instance.GetPlayerPos(askForHelp);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!boss && !catBombEnemy)
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
                if (health == 0 && bossCanDie)
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
        
        
        
        if (canAttack && !kick && !boss)
        {
          
            if (enemyLooksRight)
            {
                attackArea = transform.position;
                attackArea.x += 1f;
                attackArea.y += 0.5f;
                Instantiate(enemyAttack, attackArea, quaternion.identity);
                canAttack = false;
                kick = false;
                return;
            }
            attackArea = transform.position;
            attackArea.x += -1f;
            attackArea.y += 0.5f;
            Instantiate(enemyAttack, attackArea, quaternion.identity);
            canAttack = false;
            kick = false;
        }

        if (canAttack && boss)
        {
            attackArea = transform.position;
            Instantiate(bossAttack, attackArea, Quaternion.identity);
            canAttack = false;
        }
        
    }
}
