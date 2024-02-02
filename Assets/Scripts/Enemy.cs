using System;
using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine.SceneManagement;
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
    [SerializeField] private float bossCooldown;
    
    [SerializeField] private float enemySpeed;
    private Vector3 targetPos;
    private float counter;
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
    private int boomerDMG;
    private int playerDMG;
    private int increaseHealth = 0;

    private int currentPhase;
    private bool ZeroOnlyOnce;

    
    private bool kick;
    //public static int activeEnemies;
    
    private bool askForHelp;
    // Start is called before the first frame update
    void Awake()
    {
        ZeroOnlyOnce = true;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            increaseHealth = GameManager.Instance.GetEndlessNumber();
            increaseHealth--;
        }
        for (int i = 0; i < enemys.Length; i++)
        {
            //print(i);
            enemys[i] = GameManager.Instance.EnemyTypes()[i];
        }
        if (firstEnemy)
        {
            health = 1 + increaseHealth;
        }
        if (secondEnemy)
        {
            health = 2 + increaseHealth;
        }
        if (catGunEnemy)
        {
            health = 8 + increaseHealth;
        }
        if (fourthEnemy)
        {
            health = 4 + increaseHealth;
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
            currentPhase = 1;
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
        //print("Health:" + health);
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            playerDMG = 1;
            boomerDMG = 1;
            increaseHealth = 0;
        }
        else
        {
            playerDMG = GameManager.Instance.GetDMG();
            boomerDMG = GameManager.Instance.GetBDMG();
            increaseHealth = GameManager.Instance.GetEndlessNumber();
            if (health > 1)
            {
                increaseHealth--;
            }
        }
       
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
        
        //print($"Die Werte: CanAttack: {canAttack} EnemyLooksRight {enemyLooksRight}");
        if (!boss)
        {
            //print($"Ask for help: {askForHelp}");
            //print($"Can Attack?: {transform.position}");
            if (!catBombEnemy)
            {
                anime.SetBool("CatAttack", false);
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
                        //print("Hiya :3 Go Right");
                        enemyLooksRight = true;
                        sr.flipX = true;
                        anime.SetBool("EnemyWalking", true);
                        anime.SetBool("MaskCatWalking", true);
                        nv += new Vector2(1, 0);
                    }
            
                    if (targetPos.x + 1f < transform.position.x)
                    {
                        //print("Hiya :3 Go Left");
                        enemyLooksRight = false;
                        sr.flipX = false;
                        anime.SetBool("EnemyWalking", true);
                        anime.SetBool("MaskCatWalking", true);
                        nv += new Vector2(-1, 0);
                    }

                    if (catGunEnemy)
                    {
                        if (Mathf.Abs(transform.position.x) < 7) allowWalking = false;
                    }
                }
                if (targetPos.y - 1f > transform.position.y)
                {
                    //anime.SetBool("MaskCatWalking", true);
                    nv += new Vector2(0, 1);
                }
            
                if (targetPos.y - 1f < transform.position.y || Math.Abs(targetPos.y - 1f - transform.position.y) < 0.1f)
                {
                    //anime.SetBool("MaskCatWalking", true);
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
                        anime.SetBool("CatAttack", true);
                        canAttack = true;
                        counter = 0;
                    }
                    counter += Time.deltaTime;
                }
            }
        }
        if (boss)
        {
            //print("Boss: " + health);
            bossCanDie = GameManager.Instance.GetBossCanDie();
            health = GameManager.Instance.GetBossHP;
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
            //print("Cat rnd:" + rnd);
            if (health == 0)
            {
                explosionArea = transform.position;
                explosionArea.x += 0;
                explosionArea.y += -0.5f;
                Instantiate(bomb, explosionArea, Quaternion.identity);
                Destroy(gameObject);
            }
            rnd = Random.Range(30, 60);
            //print("Cat rnd:" + rnd);
            if (bombCooldown > rnd)
            {
                health--;
                bombCooldown = 0;
            }
            bombCooldown += Time.deltaTime;
            if (Vector2.Distance(targetPos, transform.position) < 3)
            {
                explosionArea = transform.position;
                explosionArea.x += 0;
                explosionArea.y += -0.5f;
                Instantiate(bomb, explosionArea, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        
    }

    void Ask()
    {
        targetPos = GameManager.Instance.GetPlayerPos(askForHelp);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            boomerDMG = GameManager.Instance.GetBDMG();
            playerDMG = GameManager.Instance.GetDMG();
        }
        else
        {
            boomerDMG = 1;
            playerDMG = 1;
        }
        if (!boss && !catBombEnemy)
        {
            if (collision.gameObject.CompareTag("BoomerAttack"))
            {
                health -= boomerDMG;
                sr.color = hitColor;
                hit = true;
                if (health <= 0)
                {
                    GameManager.Instance.ActiveEnemiesRemove();
                    Destroy(gameObject);
                }
            }
            else if (collision.gameObject.CompareTag("Attack"))
            {
                health -= playerDMG;
                sr.color = hitColor;
                hit = true;
                if (health <= 0)
                {
                    GameManager.Instance.ActiveEnemiesRemove();
                    Destroy(gameObject);
                }
            
            }
        }

        if (boss)
        {
            if (collision.gameObject.CompareTag("Attack") || collision.gameObject.CompareTag("BoomerAttack"))
            {
                //print("Did I got him?");
                if (collision.gameObject.CompareTag("Attack"))
                {
                    health -= playerDMG;
                    GameManager.Instance.BossGotHit(playerDMG);
                }
                else
                {
                    health -= boomerDMG;
                    GameManager.Instance.BossGotHit(boomerDMG);
                }
                
                switch (health)
                {
                    case 110:
                        anime.SetBool("NextBossPhase", true);
                        break;
                    case 200:
                    case 100:
                    case 0 when ZeroOnlyOnce:
                    {
                        anime.SetBool("NextBossPhase", false);
                        bossEnter = false;
                        StartCoroutine(BossEnterAfterTime());
                        
                        ZeroOnlyOnce = false;
                        currentPhase++;
                        break;
                    }
                }
                
                sr.color = hitColor;
                hit = true;
                if (health <= 0 && bossCanDie)
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
                anime.SetBool("Shoot", false);
                return;
            }
            attackArea = transform.position;
            attackArea.x += -1f;
            attackArea.y += 0.5f;
            Instantiate(bullet, attackArea, quaternion.identity);
            canAttack = false;
            kick = false;
            anime.SetBool("Shoot", false);
        }
        
    }

    private IEnumerator BossEnterAfterTime()
    {
        yield return new WaitForSeconds(bossCooldown);
        bossEnter = true;
    }
}
