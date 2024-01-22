using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool intro;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        if (!intro)
        {
            rb.velocity = new Vector2(0, 1);
        }
        
        if (intro)
        {
            rb.velocity = new Vector2(0, -2);
        }
    }
}
