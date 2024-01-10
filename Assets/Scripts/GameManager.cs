using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;using Update = UnityEngine.PlayerLoop.Update;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    public GameObject[] enemys = new GameObject[0];

    private Vector3 playerPos;

    private int hp;
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance is not null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            DontDestroyOnLoad(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            print("I");
            GameObject newEnemy = Instantiate(enemys[0], new Vector3(7.5f,Random.Range(-5f, 1f),0), Quaternion.identity);
        }

        if (hp == 0)
        {
            print("GAME OVER");
        }
    }

    public Vector3 Chase(bool asked)
    {
        if (asked)
        {
            return playerPos;
        }
        return playerPos;
    }

    public void PlayerPos(Vector3 pos)
    {
        playerPos = pos;
    }

    public GameObject EnemyTypes()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            return enemys[i];
        }
        return enemys[0];
    }

    public void HP(int playerHealth)
    {
        hp = playerHealth;
    }

    public void PlayerGotHit()
    {
        hp--;
    }
}
