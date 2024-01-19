using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anime;
    [SerializeField] private float speed;
    [SerializeField] private GameObject attack;
    [SerializeField] private GameObject attackBR;
    [SerializeField] private bool player;
    private Vector3 playerPos;
    private Vector3 attackArea;
    private bool canAttack = true;
    private float counter;
    private float cooldown;
    private bool playerLooksRight;
    private int currentSceneIndex;
    private bool alive;
    private float inputH;
    private float inputV;
    private bool sendHP;
    private float boomerangCooldown;
    private bool boomerang;
    private bool startCooldown;
    private bool hit;
    private float hitCooldown;
    private Color hitColor = new Color(1f, 100f / 255f, 100 / 255f, 1f);

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        boomerang = false;
        startCooldown = true;
        boomerangCooldown = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
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
            inputH = Input.GetAxis("Horizontal");
            inputV = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(inputH * speed, inputV * speed);
            if (inputH < 0f)
            {
                playerLooksRight = false;
                sr.flipX = false;
                anime.SetBool("IsWalking", true);
                //bool false
            }
            else if (inputH > 0)
            {
                playerLooksRight = true;
                sr.flipX = true;
                anime.SetBool("IsWalking", true);
                //bool true
            }
            else
            { 
                anime.SetBool("IsWalking", false);
            }
            /*if (Input.GetKeyDown(KeyCode.W))
            {
                rb.velocity = new Vector2(0, 1);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                playerLooksRight = false;
                rb.velocity = new Vector2(-1, 0);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                rb.velocity = new Vector2(0, -1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                playerLooksRight = true;
                rb.velocity = new Vector2(1, 0);
            }*/
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                anime.SetBool("Punch", true);
                if (canAttack)
                {
                    if (playerLooksRight)
                    {
                        canAttack = false;
                        attackArea = playerPos;
                        attackArea.x += 1.3f;
                        attackArea.y += 0.8f;
                        sr.flipX = true;
                        Instantiate(attack, attackArea, quaternion.identity);
                    }
                    else
                    {
                        canAttack = false;
                        attackArea = playerPos;
                        attackArea.x += -1.3f;
                        attackArea.y += 0.8f;
                        sr.flipX = false;
                        Instantiate(attack, attackArea, quaternion.identity);   
                    }
                }
            }
            
            if (Input.GetKeyDown(KeyCode.K))
            {
                
                anime.SetBool("Kick", true);
                if (canAttack)
                {
                    if (playerLooksRight)
                    {
                        canAttack = false;
                        attackArea = playerPos;
                        attackArea.x += 1f;
                        attackArea.y += -1f;
                        Instantiate(attack, attackArea, quaternion.identity);
                    }
                    else
                    {
                        canAttack = false;
                        attackArea = playerPos;
                        attackArea.x += -1f;
                        attackArea.y += -1f;
                        Instantiate(attack, attackArea, quaternion.identity);   
                    }
                }
            }
           
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (boomerang)
                {
                    //boomerang = false;
                    attackArea = playerPos;
                    
                    if (playerLooksRight)
                    {
                        attackArea.x += 1.5f;
                    }
                    else
                    {
                        attackArea.x += -1.5f;
                    }
                    
                    Instantiate(attackBR, attackArea, Quaternion.identity);
                }

                boomerang = false;
            }
            
            playerPos = transform.position;
            
            if (!canAttack)
            {
                if (counter > 0.2)
                {
                    canAttack = true;
                    anime.SetBool("Punch", false);
                    anime.SetBool("Kick", false);
                    counter = 0;
                }
                counter += Time.deltaTime;
            }
            if (!boomerang && startCooldown)
            {
                GameManager.Instance.GiveBoomerCooldown(boomerangCooldown);
                if (boomerangCooldown <= 0)
                {
                    boomerang = true;
                    startCooldown = false;
                    boomerangCooldown = 1;
                }
                boomerangCooldown -= Time.deltaTime;
            }
        }

        
    }

    private void FixedUpdate()
    {
        if (currentSceneIndex >= 1)
        {
            if (player)
            {
                GameManager.Instance.PlayerPos(transform.position); 
            }   
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            sr.color = hitColor;
            hit = true;
        }
        
        if (collision.CompareTag("BOOMERang"))
        {
            startCooldown = true;
        }
    }
}
