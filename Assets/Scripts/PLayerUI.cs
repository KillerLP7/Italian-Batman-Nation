using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerUI : MonoBehaviour
{
    private Animator anime;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        anime.SetBool("Batman1", false);
        anime.SetBool("Batman1", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Level 3"))
        {
            anime.SetBool("Batman1", true);
        }
        if (collision.CompareTag("Level Boss"))
        {
            anime.SetBool("Batman2", true);
        }
    }
}
