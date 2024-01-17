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
    private bool a;
    private bool d;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        boomerang = false;
        startCooldown = true;
        boomerangCooldown = 30;
        a = true;
        d = true;

        //Time.timeScale = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            inputH = Input.GetAxis("Horizontal");
            inputV = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(inputH * speed, inputV * speed);
            if (Input.GetKeyDown(KeyCode.A))
            {
                playerLooksRight = false;
                sr.flipX = false;
                anime.SetBool("IsWalking", true);
                //bool false
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                //TODO: No Moonwalking in my House!
                playerLooksRight = true;
                sr.flipX = true;
                anime.SetBool("IsWalking", true);
                //bool true
            }
            if (rb.velocity.x == 0 && rb.velocity.y == 0)
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
                        attackArea.x += 1f;
                        attackArea.y += 0.5f;
                        sr.flipX = true;
                        Instantiate(attack, attackArea, quaternion.identity);
                    }
                    else
                    {
                        canAttack = false;
                        attackArea = playerPos;
                        attackArea.x += -1f;
                        attackArea.y += 0.5f;
                        sr.flipX = false;
                        Instantiate(attack, attackArea, quaternion.identity);   
                    }
                }
                anime.SetBool("Kick", false);
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
                        attackArea.y += -1.5f;
                        Instantiate(attack, attackArea, quaternion.identity);
                    }
                    else
                    {
                        canAttack = false;
                        attackArea = playerPos;
                        attackArea.x += -1f;
                        attackArea.y += -1.5f;
                        Instantiate(attack, attackArea, quaternion.identity);   
                    }
                }
                anime.SetBool("Kick", false);
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
                if (counter > 0.5)
                {
                    canAttack = true;
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
                    boomerangCooldown = 30;
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
        if (collision.CompareTag("BOOMERang"))
        {
            startCooldown = true;
        }
    }
}
