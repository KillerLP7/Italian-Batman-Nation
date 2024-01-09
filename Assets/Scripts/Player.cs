using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 100f;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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
    }

    private void FixedUpdate()
    {
        GameManager.Instance.PlayerPos(transform.position);
    }
}
