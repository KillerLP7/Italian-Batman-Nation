using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toxic : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anime;
    [SerializeField] private GameObject attack;
    private float counter;
    private bool dmg;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        anime.SetBool("ToxicBallSpawn", true);
    }

    // Update is called once per frame
    void Update()
    {
        anime.SetBool("ToxicBallSpawn", false);
        if (!dmg)
        {
            anime.SetBool("ToxicDmg", false);
            dmg = true;
        }
        rb.velocity = new Vector2(-2, 0);
        if (counter > 0.5)
        {
            anime.SetBool("ToxicDmg", true);
            dmg = false;
            Instantiate(attack, transform.position, Quaternion.identity);
            counter = 0;
        }
        counter += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destroy"))
        {
            Destroy(gameObject);
        }
    }
}
