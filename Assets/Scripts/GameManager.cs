using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    public GameObject[] enemys;
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI bossHealth;

    private Vector3 playerPos;
    private int waveNumber = 14;
    private int hp;
    private int activeEnemies;
    private bool endOfWave;
    private bool[] binary = new bool[4];
    private int lastWave;
    private bool spawnBoss;
    private int bossHP = 30;

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

        for (int i = 0; i < binary.Length; i++)
        {
            binary[i] = false;
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(3);
        }

        if (allowSpawn)
        {
            GetBinary();
            //print($"try to spawn with {string.Join(", ", binary)}");
            if (binary[0])
            {
                GameObject cat1 = Instantiate(enemys[0], new Vector3(Random.Range(10f, 15f), Random.Range(-5f, 1f), 0), Quaternion.identity);
            }

            if (binary[1])
            {
                GameObject cat2 = Instantiate(enemys[1], new Vector3(Random.Range(10f, 15f), Random.Range(-5f, 1f), 0), Quaternion.identity);
            }

            if (binary[2])
            {
                GameObject cat4 = Instantiate(enemys[2], new Vector3(Random.Range(10f, 15f), Random.Range(-5f, 1f), 0), Quaternion.identity);
            }

            if (binary[3])
            {
                GameObject cat8 = Instantiate(enemys[3], new Vector3(Random.Range(10f, 15f), Random.Range(-5f, 1f), 0), Quaternion.identity);
            }
            //GameObject cat2 = Instantiate(enemys[0], new Vector3(Random.Range(10f, 15f),Random.Range(-5f, 1f),0), Quaternion.identity);
            //GameObject cat4 = Instantiate(enemys[0], new Vector3(Random.Range(10f, 15f),Random.Range(-5f, 1f),0), Quaternion.identity);
            //GameObject cat8 = Instantiate(enemys[0], new Vector3(Random.Range(10f, 15f),Random.Range(-5f, 1f),0), Quaternion.identity);
            //GameObject catBoss = Instantiate(enemys[0], new Vector3(Random.Range(10f, 15f),Random.Range(-5f, 1f),0), Quaternion.identity);


        }

        allowSpawn = false;
        
        print(activeEnemies);
        playerHealth.text = hp.ToString();
        wave.text = waveNumber.ToString();

        if (activeEnemies == 0 && !endOfWave)
        {
            allowSpawn = true;
            if (waveNumber < 16)
            {
                spawnBoss = false;
                lastWave = waveNumber;
                waveNumber++;
                //print("Incerased Wave");
            }
            endOfWave = true;
        }
        if (waveNumber > 15)
        {
            wave.text = "BOSS";
            bossHealth.text = bossHP.ToString();
            spawnBoss = true;
        }

        if (allowSpawn && spawnBoss)
        {
            Instantiate(enemys[4], new Vector3(8.88888f, 0, 0), quaternion.identity);
            allowSpawn = false;
        }
        if (activeEnemies > 0)
        {
            endOfWave = false;
        }
        if (hp == 0)
        {
            print("GAME OVER");
        }

        if (bossHP == 0)
        {
            SceneManager.LoadScene(0);
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

    public int GetWaveNumber()
    {
        return waveNumber;
    }
    
    private void GetBinary()
    {
        int tmp = waveNumber;
        for (int i = 0; i < binary.Length; i++)
        {
            binary[i] = tmp % 2 == 1;
            tmp >>= 1;
        }
    }

    public bool AmIDead()
    {
        bool isThePlayerAlive = hp != 0;
        return isThePlayerAlive;
    }

    public void BossGotHit()
    {
        bossHP--;
    }
}
