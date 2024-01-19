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
    [SerializeField] private GameObject bullet;
    [SerializeField] private bool firstEnemy;
    [SerializeField] private GameObject bomb;
    [SerializeField] private bool secondEnemy;
    [SerializeField] private bool catGunEnemy;
    [SerializeField] private bool fourthEnemy;
    [SerializeField] private bool catBombEnemy;
    [SerializeField] private bool boss;
    [SerializeField] private Color hitColor = new Color(1f, 100f / 255f, 100 / 255f, 1f);
    
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
    private bool hit;
    private float hitCooldown;
    private bool allowWalking;
    
    private bool firstTimeTwoHundred;
    private bool firstTimeHundred;
    private bool firstTimeZero;

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
        if (catGunEnemy)
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
            firstTimeHundred = firstTimeZero = firstTimeTwoHundred = true;
        }

        canAttack = true;
        askForHelp = true;
        enemyLooksRight = true;
        allowWalking = true;
        
        Ask();
        GameManager.Instance.ActiveEnemiesAdd();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (hit)
        {
            if (hitCooldown > 0.1)
            {
                sr.color = Color.white;
                hit = false;
                hitCooldown = 0;
            }
            hitCooldown += Time.deltaTime;
        }
        
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
                anime.SetBool("MaskCatWalking", false);
            }
            else if (targetPos.y < transform.position.y)
            {
                nv += new Vector2(0, 0f);
                anime.SetBool("EnemyWalking", false);
                anime.SetBool("MaskCatWalking", false);
            }
            if (targetPos.y < transform.position.y || targetPos.y > transform.position.y)
            {
                // && allowWalking || !catGunEnemy
                if (allowWalking)
                {
                    if (targetPos.x - 1f > transform.position.x)
                    {
                        print("Hiya :3 Go Right");
                        enemyLooksRight = true;
                        sr.flipX = true;
                        anime.SetBool("EnemyWalking", true);
                        nv += new Vector2(1, 0);
                    }
            
                    if (targetPos.x + 1f < transform.position.x)
                    {
                        print("Hiya :3 Go Left");
                        enemyLooksRight = false;
                        sr.flipX = false;
                        anime.SetBool("EnemyWalking", true);
                        nv += new Vector2(-1, 0);
                    }

                    if (catGunEnemy)
                    {
                        if (Mathf.Abs(transform.position.x) < 5) allowWalking = false;
                    }
                }
                if (targetPos.y - 1f > transform.position.y)
                {
                    anime.SetBool("MaskCatWalking", true);
                    nv += new Vector2(0, 1);
                }
            
                if (targetPos.y - 1f < transform.position.y || Math.Abs(targetPos.y - 1f - transform.position.y) < 0.1f)
                {
                    anime.SetBool("MaskCatWalking", true);
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
            print("Boss: " + health);
            bossCanDie = GameManager.Instance.GetBossCanDie();
            health = GameManager.Instance.GetBossHP;
            GameManager.Instance.BossPhase();
            if (!canAttack)
            {
                if (counter > 0.1)
                {
                    anime.SetBool("Shoot", false);
                    canAttack = true;
                    counter = 0;
                }
                counter += Time.deltaTime;
            }
            ExecuteAttack();
            switch (health)
            {
                case 110:
                    anime.SetBool("NextBossPhase", true);
                    break;
                case 200 when firstTimeTwoHundred:
                case 100 when firstTimeHundred:
                case 0 when firstTimeZero:
                {
                    anime.SetBool("NextBossPhase", false);
                    //TODO: Execute only once
                    if (bossEnter && transform.position.x < 9f)
                    {
                        bossEnter = false;
                    }
                    
                    if (firstTimeZero)
                    {
                        firstTimeZero = false;
                    }
                    else if (firstTimeTwoHundred)
                    {
                        firstTimeTwoHundred = false;
                    }
                    else if (firstTimeHundred)
                    {
                        firstTimeHundred = false;
                    }

                    break;
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
                sr.color = hitColor;
                hit = true;
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
                sr.color = hitColor;
                hit = true;
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
        
        
        if (canAttack && kick && !catGunEnemy)
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
        
        
        
        if (canAttack && !kick && !boss && !catGunEnemy)
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

        if (canAttack && boss && !catGunEnemy)
        {
            attackArea = transform.position;
            Instantiate(bossAttack, attackArea, Quaternion.identity);
            canAttack = false;
        }

        if (canAttack && catGunEnemy)
        {
            anime.SetBool("Shoot", true);
            if (enemyLooksRight)
            {
                attackArea = transform.position;
                attackArea.x += 1f;
                attackArea.y += 0.5f;
                Instantiate(bullet, attackArea, quaternion.identity);
                canAttack = false;
                kick = false;
                return;
            }
            attackArea = transform.position;
            attackArea.x += -1f;
            attackArea.y += 0.5f;
            Instantiate(bullet, attackArea, quaternion.identity);
            canAttack = false;
            kick = false;
        }
        
    }
}
