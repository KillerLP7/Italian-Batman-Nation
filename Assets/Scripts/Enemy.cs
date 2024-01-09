using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject[] enemys = new GameObject[0];

    Rigidbody2D rb;
    private Vector3 targetPos;

    private bool knowYourLocation;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 nv = new Vector2();
        
        //rb.velocity = new Vector2(-1, 0);
        targetPos = GameManager.Instance.Chase();
        if (targetPos.y > transform.position.y)
        {
            //nv += new Vector2(0, 1);
            rb.velocity = new Vector2(0, 1);
        }

        if (targetPos.y < transform.position.y)
        {
            //nv += new Vector2(0, -1);
            rb.velocity = new Vector2(0, -1);
        }
        
        if (Math.Abs(targetPos.y - transform.position.y) < 0.1f)
        {
            if (targetPos.x > transform.position.x)
            {
                //nv += new Vector2(1, 0);
                rb.velocity = new Vector2(1, 0);
            }
            
            if (targetPos.x < transform.position.x)
            {
                //nv += new Vector2(-1, 0);
                rb.velocity = new Vector2(-1, 0);
            }

            
            
            if (targetPos.x == transform.position.x)
            {
                print("GAME OVER");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameObject newEnemy = Instantiate(enemys[0], new Vector3(4,0,0), Quaternion.identity);
        }
        
        //rb.velocity = nv;
    }
}
