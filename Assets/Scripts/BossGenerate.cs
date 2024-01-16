using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerate : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool canBossDie;
    private bool bossSpawned;
    private Vector3 firstSpawn;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        canBossDie = false;
        bossSpawned = false;
        firstSpawn = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //canBossDie = GameManager.Instance.GetBossCanDie();
        //bossSpawned = GameManager.Instance.BossSpawned();
        if (!canBossDie && bossSpawned)
        {
            if (transform.position.x > 0)
            {
                rb.velocity = new Vector2(-1, 0);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

        if (canBossDie && bossSpawned)
        {
            if (transform.position.x < firstSpawn.x)
            {
                rb.velocity = new Vector2(1, 0);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}
