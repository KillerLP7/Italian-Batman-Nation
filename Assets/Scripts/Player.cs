using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    private float speed = 100f;
    [SerializeField] private GameObject attack;
    [SerializeField] private bool player;
    private Vector3 playerPos;
    private Vector3 attackArea;
    private bool canAttack = true;
    private float counter;
    private float cooldown;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
                rb.velocity = new Vector2(-1, 0);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                rb.velocity = new Vector2(0, -1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                rb.velocity = new Vector2(1, 0);
            }
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (canAttack)
                {
                    canAttack = false;
                    attackArea = playerPos;
                    attackArea.x += 1f;
                    attackArea.y += 0.5f;
                    Instantiate(attack, attackArea, quaternion.identity);
                
                }
            }
            
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (canAttack)
                {
                    attackArea = playerPos;
                    attackArea.x += 1f; 
                    attackArea.y += -0.5f;
                    Instantiate(attack, attackArea, quaternion.identity);
                    canAttack = false;
                }
            }
            
            playerPos = transform.position;
            
            if (!canAttack)
            {
                canAttack = true;
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
        if (player)
        {
            GameManager.Instance.PlayerPos(transform.position); 
        }
    }

    private void ExecuteAttack()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Hit!");
    }
}
