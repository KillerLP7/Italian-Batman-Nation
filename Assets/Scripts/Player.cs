using System;
using TMPro;
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
    public TextMeshProUGUI pressP;
    public TextMeshProUGUI pressK;
    public TextMeshProUGUI pressWASD;
    public TextMeshProUGUI pressSpacebar;
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
    private bool unlocked;
    private bool lvl3;
    private bool lvlBoss;
    //static public bool tutorial;
    //static public bool tutorialWASD;
    static public bool tutorialP;
    static public bool tutorialK;
    static public bool tutorialBoomer;
    private int time;
    
    //static public bool tutorialSpacebar;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        boomerang = false;
        startCooldown = true;
        boomerangCooldown = 0;
        anime.SetBool("Batman1", false);
        anime.SetBool("Batman2", false);
        unlocked = false;
        lvl3 = true;
        lvlBoss = true;
        if (currentSceneIndex < 1)
        {
            tutorialP = true;
            tutorialK = true;
            pressP.enabled = true;
            pressK.enabled = false;
            pressWASD.enabled = false;
        }
        else
        {
            tutorialP = false;
            tutorialK = false;
            pressSpacebar.enabled = false;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += FindText;
    }

    private void FindText(Scene arg0, LoadSceneMode arg1)
    {
        if (currentSceneIndex > 1)
        {
            pressSpacebar = GameObject.FindGameObjectWithTag("SpaceTutorial").GetComponent<TextMeshProUGUI>();
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= FindText;
    }

    // Update is called once per frame
    void Update()
    {
        time = PlayerPrefs.GetInt(Options.speedKey, 2);
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

            if (!tutorialK)
            {
                inputH = Input.GetAxis("Horizontal");
                inputV = Input.GetAxis("Vertical");
                if (inputH > 0 || inputV > 0)
                {
                    Destroy(pressWASD);
                }
            }
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
                if (tutorialP)
                {
                    tutorialP = false;
                    pressK.enabled = true;
                    Destroy(pressP);
                }
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
            
            if (Input.GetKeyDown(KeyCode.K) && !tutorialP)
            {
                if (tutorialK)
                {
                    tutorialK = false;
                    pressWASD.enabled = true;
                    Destroy(pressK);
                }
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
           
            
            if (Input.GetKeyDown(KeyCode.Space) && unlocked)
            {
                switch (time)
                {
                    case 0:
                        Time.timeScale = 0.5f;
                        break;
                    case 1:
                        Time.timeScale = 0.75f;
                        break;
                    case 2:
                        Time.timeScale = 1;
                        break;
                    case 3:
                        Time.timeScale = 1.5f;
                        break;
                    case 4:
                        Time.timeScale = 2;
                        break;
                }
                pressSpacebar.enabled = false;
                if (boomerang)
                {
                    boomerang = false;
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
                if (currentSceneIndex >= 1)
                {
                    GameManager.Instance.GiveBoomerCooldown(boomerangCooldown);
                }
                if (boomerangCooldown <= 0.1f)
                {
                    boomerang = true;
                    startCooldown = false;
                    boomerangCooldown = 0;
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

        if (collision.CompareTag("Level 2"))
        {
            print("Lets switch to Level 2!");
            if (!unlocked)
            {
                unlocked = true;
                pressSpacebar.enabled = true;
                Time.timeScale = 0;
            }
        }
        if (collision.CompareTag("Level 3"))
        {
            print("Lets switch to Level 3!");
            if (lvl3)
            {
                anime.SetBool("Batman1", true);
                GameManager.Instance.Armor(5);
                lvl3 = false;
            }
        }
        if (collision.CompareTag("Level Boss"))
        {
            print("Lets switch to Level Boss!");
            if (lvlBoss)
            {
                anime.SetBool("Batman2", true);
                GameManager.Instance.Armor(15);
                lvlBoss = false;
            }
        }
    }
}
