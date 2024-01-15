using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    private float speed = 2f;
    [SerializeField] private GameObject attack;
    [SerializeField] private GameObject attackBR;
    [SerializeField] private bool player;
    private SpriteRenderer sr;
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
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        boomerang = true;
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
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                playerLooksRight = true;
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
                }
            }
            
            playerPos = transform.position;
            
            if (!canAttack)
            {
                if (counter > 1)
                {
                    canAttack = true;
                    counter = 0;
                }
                counter += Time.deltaTime;
            }
            if (!boomerang)
            {
                if (boomerangCooldown > 1)
                {
                    canAttack = true;
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
