using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    private float counter;
    private void Update()
    {
        if (counter > 0.1)
        {
            Destroy(gameObject);
            counter = 0;
        }
        counter += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Sprite attack
            //Player Health Bar drops
            //print("We got him!");
            GameManager.Instance.PlayerGotHit();
        }
    }
}
