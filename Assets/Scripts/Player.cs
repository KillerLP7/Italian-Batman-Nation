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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        boomerang = true;
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
                if (canAttack)
                {
                    if (playerLooksRight)
                    {
                        canAttack = false;
                        attackArea = playerPos;
                        attackArea.x += 1f;
                        attackArea.y += 0.5f;
                        Instantiate(attack, attackArea, quaternion.identity);
                    }
                    else
                    {
                        canAttack = false;
                        attackArea = playerPos;
                        attackArea.x += -1f;
                        attackArea.y += 0.5f;
                        Instantiate(attack, attackArea, quaternion.identity);   
                    }
                }
            }
            
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (canAttack)
                {
                    if (playerLooksRight)
                    {
                        canAttack = false;
                        attackArea = playerPos;
                        attackArea.x += 1f;
                        attackArea.y += -0.5f;
                        Instantiate(attack, attackArea, quaternion.identity);
                    }
                    else
                    {
                        canAttack = false;
                        attackArea = playerPos;
                        attackArea.x += -1f;
                        attackArea.y += -0.5f;
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
                        attackArea.x += 1f;
                    }
                    else
                    {
                        attackArea.x += -1f;
                    }
                    
                    Instantiate(attackBR, attackArea, Quaternion.identity);
                    boomerang = false;
                }
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
            if (!boomerang)
            {
                GameManager.Instance.GiveBoomerCooldown(boomerangCooldown);
                if (boomerangCooldown > 30)
                {
                    boomerang = true;
                    boomerangCooldown = 0;
                }
                boomerangCooldown += Time.deltaTime;
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
}
