using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    private float speed = 100f;
    [SerializeField] private GameObject attack;
    [SerializeField] private bool player;
    private SpriteRenderer sr;
    private Vector3 playerPos;
    private Vector3 attackArea;
    private bool canAttack = true;
    private float counter;
    private float cooldown;
    private int hp = 5;
    private bool playerLooksRight;
    private int currentSceneIndex;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex >= 1)
        {
            GameManager.Instance.HP(hp);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        if (player)
        {
            if (Input.GetKeyDown(KeyCode.W))
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
            }
            
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
        }

        if (!player)
        {
            if (counter > 0.1)
            {
                ExecuteAttack();
                counter = 0;
            }
            counter += Time.deltaTime;
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

    private void ExecuteAttack()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                print("Hit!");
            }
        }
    }
}
