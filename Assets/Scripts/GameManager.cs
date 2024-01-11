using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Update = UnityEngine.PlayerLoop.Update;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    public GameObject[] enemys;
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI wave;

    private Vector3 playerPos;
    private int waveNumber;
    private int hp;
    private int activeEnemies;
    private bool endOfWave;

    private bool allowSpawn;
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
            GameObject newEnemy = Instantiate(enemys[0], new Vector3(10f,Random.Range(-5f, 1f),0), Quaternion.identity);
        }

        for (int i = 0; i < waveNumber; i++)
        {
            if (allowSpawn)
            {
                print("try to spawn");
                GameObject newEnemy = Instantiate(enemys[0], new Vector3(Random.Range(10f, 15f),Random.Range(-5f, 1f),0), Quaternion.identity);
                
            }
        }
        allowSpawn = false;
        
        print(activeEnemies);
        playerHealth.text = hp.ToString();
        wave.text = waveNumber.ToString();

        if (activeEnemies == 0 && !endOfWave)
        {
            allowSpawn = true;
            waveNumber++;
            endOfWave = true;
        }

        if (activeEnemies > 0)
        {
            endOfWave = false;
            if (waveNumber > 5)
            {
                
            }
        }
        if (hp == 0)
        {
            print("GAME OVER");
        }
    }

    public Vector3 Chase(bool asked) => playerPos;

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

    public void ActiveEnemiesAdd()
    {
        activeEnemies++;
    }
    
    public void ActiveEnemiesRemove()
    {
        activeEnemies--;
    }
}
