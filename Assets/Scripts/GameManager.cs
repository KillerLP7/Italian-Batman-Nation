using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject[] enemys;
    public GameObject toxicBall;
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI bossHealth;
    public TextMeshProUGUI bossUI;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI cooldownNumber;

    private Vector3 playerPos;
    private int waveNumber = 15;
    public int hp;
    private int activeEnemies;
    private bool endOfWave;
    private bool[] binary = new bool[4];
    private bool spawnBoss;
    private int bossHP = 30;
    private int rnd;
    private float spawn;
    private float spawnCooldown;
    private bool boss;
    private bool bossCanDie;
    private float bossHPCounter;
    private float bossHPCooldown;
    private bool bossMaxHealth;

    private bool inMenu;
    private bool allowSpawn;
    private bool BossAllowSpawn;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < binary.Length; i++)
        {
            binary[i] = false;
        }

        bossUI.enabled = false;
        bossHealth.enabled = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Refresh;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Refresh;
    }

    private void Refresh(Scene s, LoadSceneMode m)
    {
        hp = 100;
        activeEnemies = 0;
        bossCanDie = false;
        playerHealth.enabled = true;
        wave.enabled = true;
        playerHealthText.enabled = true;
        waveText.enabled = true;
        bossHPCounter = 5;
        bossMaxHealth = false;
        cooldownText.enabled = true;
        cooldownNumber.enabled = true;
        if (waveNumber != 0)
        {
            waveNumber--;
        }
        inMenu = SceneManager.GetActiveScene().buildIndex != 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (inMenu)
        {
            playerHealth.enabled = false;
            wave.enabled = false;
            playerHealthText.enabled = false;
            waveText.enabled = false;
            bossHealth.enabled = false;
            bossUI.enabled = false;
            cooldownText.enabled = false;
            cooldownNumber.enabled = false;
            return;
        } 
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            print("I");
            Instantiate(enemys[0], new Vector3(10f,Random.Range(-5f, 1f),0), Quaternion.identity);
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
                rnd = Random.Range(0, 2);
                if (rnd == 0)
                {
                    spawn = -15f;
                }
                if (rnd == 1)
                {
                    spawn = 15f;
                }
                Instantiate(enemys[0], new Vector3(spawn, Random.Range(-5f, 1f), 0), Quaternion.identity);
            }

            if (binary[1])
            {
                rnd = Random.Range(0, 2);
                if (rnd == 0)
                {
                    spawn = -15f;
                }
                if (rnd == 1)
                {
                    spawn = 15f;
                }
                Instantiate(enemys[1], new Vector3(spawn, Random.Range(-5f, 1f), 0), Quaternion.identity);
            }

            if (binary[2])
            {
                rnd = Random.Range(0, 2);
                if (rnd == 0)
                {
                    spawn = -15f;
                }
                if (rnd == 1)
                {
                    spawn = 15f;
                }
                Instantiate(enemys[2], new Vector3(spawn, Random.Range(-5f, 1f), 0), Quaternion.identity);
            }

            if (binary[3])
            {
                rnd = Random.Range(0, 2);
                if (rnd == 0)
                {
                    spawn = -15f;
                }
                if (rnd == 1)
                {
                    spawn = 15f;
                }
                Instantiate(enemys[3], new Vector3(spawn, Random.Range(-5f, 1f), 0), Quaternion.identity);
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
                waveNumber++;
                //print("Incerased Wave");
            }
            endOfWave = true;
        }

        if (boss)
        {
            BossAllowSpawn = false;
            if (spawnCooldown > 15)
            {
                BossAllowSpawn = true;
                spawnCooldown = 0;
            }
            spawnCooldown += Time.deltaTime;
        }
        if (waveNumber > 15)
        {
            wave.text = "BOSS";
            bossUI.enabled = true;
            bossHealth.enabled = true;
            bossHealth.text = bossHP.ToString();
            spawnBoss = true;
        }

        if (allowSpawn && spawnBoss)
        {
            Instantiate(enemys[4], new Vector3(15f, 0, 0), quaternion.identity);
            allowSpawn = false;
            boss = true;
        }
        if (activeEnemies > 0)
        {
            endOfWave = false;
        }
        if (hp <= 0)
        {
            print("GAME OVER");
            SceneManager.LoadScene(0);
        }

        if (bossHP == 0 && bossCanDie)
        {
            SceneManager.LoadScene(0);
        }

        if (!bossCanDie)
        {
            if (bossHP <= 3 || bossMaxHealth)
            {
                bossMaxHealth = true;
                if (bossHPCooldown > bossHPCounter)
                {
                    bossHP++;
                    bossHPCounter -= 0.5f;
                    bossHPCooldown = 0;
                }
                bossHPCooldown += Time.deltaTime;
            }
        }

        if (bossHP >= 300)
        {
            bossCanDie = true;
            bossMaxHealth = false;

        }
    }

    public Vector3 GetPlayerPos(bool asked) => playerPos;

    public void PlayerPos(Vector3 pos)
    {
        playerPos = pos;
    }

    public GameObject[] EnemyTypes() => enemys;

    public void PlayerGotHit()
    {
        //print($"Player got hit!");
        hp--;
    }

    public void ActiveEnemiesAdd()
    {
        activeEnemies++;
    }
    
    public void ActiveEnemiesRemove()
    {
        activeEnemies--;
        hp++;
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

    public void BossGotHit()
    {
        bossHP--;
    }

    public void BossPhase()
    {
        //Fake Death 30 -> 300
        //Spawn Bombcats, Toxicball, Enemy Wave Spawn

        if (bossCanDie)
        {
            if (bossHP <= 100)
            {
                //Toxicball
                if (BossAllowSpawn)
                {
                    Instantiate(toxicBall, new Vector3(15f, Random.Range(1, -4), 0), Quaternion.identity);
                    Instantiate(toxicBall, new Vector3(15f, Random.Range(1, -4), 0), Quaternion.identity);
                }
            }
            if (bossHP <= 200)
            {
                //Bombcats
                if (BossAllowSpawn)
                {
                    rnd = Random.Range(0, 2);
                    if (rnd == 0)
                    {
                        spawn = -15f;
                    }
                    if (rnd == 1)
                    {
                        spawn = 15f;
                    }
                    Instantiate(enemys[5], new Vector3(spawn, Random.Range(-1, -4), 0), Quaternion.identity);
                    rnd = Random.Range(0, 2);
                    if (rnd == 0)
                    {
                        spawn = -15f;
                    }
                    if (rnd == 1)
                    {
                        spawn = 15f;
                    }
                    Instantiate(enemys[5], new Vector3(spawn, Random.Range(-1, -4), 0), Quaternion.identity);
                    rnd = Random.Range(0, 2);
                    if (rnd == 0)
                    {
                        spawn = -15f;
                    }
                    if (rnd == 1)
                    {
                        spawn = 15f;
                    }
                    Instantiate(enemys[5], new Vector3(spawn, Random.Range(-1, -4), 0), Quaternion.identity);
                    rnd = Random.Range(0, 2);
                    if (rnd == 0)
                    {
                        spawn = -15f;
                    }
                    if (rnd == 1)
                    {
                        spawn = 15f;
                    }
                    Instantiate(enemys[5], new Vector3(spawn, Random.Range(-1, -4), 0), Quaternion.identity);
                }
            }
            if (bossHP <= 300)
            {
                if (BossAllowSpawn)
                {
                    //Enemy Wave Spawn
                    rnd = Random.Range(0, 2);
                    if (rnd == 0)
                    {
                        spawn = -15f;
                    }
                    if (rnd == 1)
                    {
                        spawn = 15f;
                    }
                    Instantiate(enemys[0], new Vector3(spawn, Random.Range(-5f, 1f), 0), Quaternion.identity);
                    rnd = Random.Range(0, 2);
                    if (rnd == 0)
                    {
                        spawn = -15f;
                    }
                    if (rnd == 1)
                    {
                        spawn = 15f;
                    }
                    Instantiate(enemys[1], new Vector3(spawn, Random.Range(-5f, 1f), 0), Quaternion.identity);
                    rnd = Random.Range(0, 2);
                    if (rnd == 0)
                    {
                        spawn = -15f;
                    }
                    if (rnd == 1)
                    {
                        spawn = 15f;
                    }
                    Instantiate(enemys[2], new Vector3(spawn, Random.Range(-5f, 1f), 0), Quaternion.identity);
                    rnd = Random.Range(0, 2);
                    if (rnd == 0)
                    {
                        spawn = -15f;
                    }
                    if (rnd == 1)
                    {
                        spawn = 15f;
                    }
                    Instantiate(enemys[3], new Vector3(spawn, Random.Range(-5f, 1f), 0), Quaternion.identity);
                    BossAllowSpawn = false;
                }
            }
        }
        
    }

    public int GetBossHP()
    {
        return bossHP;
    }

    public bool GetBossCanDie()
    {
        return bossCanDie;
    }

    public void GiveBoomerCooldown(float currentCooldown)
    {
        cooldownNumber.text = currentCooldown.ToString("0");
    }

    public bool BossSpawned()
    {
        return spawnBoss;
    }
}
