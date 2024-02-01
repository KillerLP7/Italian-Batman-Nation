using System;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool endlessMode;
    
    public const string endlessModeKey = "Endless";
    private static int endlessModeBinary;

    [SerializeField] private Upgrade upgradeScreen;
    
    public GameObject[] enemys;
    public GameObject toxicBall;
    public GameObject gameOver;
    public GameObject ui;
    public GameObject uiBoss;
    public GameObject uiBoomer;
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI bossHealth;
    public TextMeshProUGUI bossUI;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI cooldownNumber;
    private Vector3 playerPos;
    private int waveNumber;
    private int lastWaveNumber;
    public int hp;
    public int lastHp;
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
    private float bossMaxHP;
    private bool bossReachedMaxHealth;
    private bool bossLevel;
    private int currentDiff;
    private int time;
    private bool isOpen;
    private int endlessWave;
    private bool resetBCD;
    private int increaseBDMG = 1;
    private int increaseDMG = 1;
    private int healthPerKill = 1;

    private bool inMenu;
    private bool allowSpawn;
    private bool BossAllowSpawn;
    // Start is called before the first frame update
    void Awake()
    {
        //OPTIONS: Speed, Difficulty * player/enemy Health, Sound
        //Time.timeScale = 2;
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
        
        endlessMode = PlayerPrefs.GetInt(endlessModeKey, 0) == 1;

        bossUI.enabled = false;
        bossHealth.enabled = false;
        bossLevel = false;
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
        if (lastHp <= 0)
        {
            lastHp = 5;
        }
        activeEnemies = 0;
        bossHP = 30;
        isOpen = false;
        bossCanDie = false;
        uiBoss.SetActive(false);
        uiBoomer.SetActive(false);
        playerHealth.enabled = true;
        bossUI.enabled = false;
        bossHealth.enabled = false;
        wave.enabled = true;
        playerHealthText.enabled = true;
        waveText.enabled = true;
        bossHPCounter = 5;
        bossReachedMaxHealth = false;
        cooldownText.enabled = true;
        cooldownNumber.enabled = true;
        gameOver.SetActive(false);
        ui.SetActive(true);
        lastWaveNumber = 1;
        waveNumber = lastWaveNumber;
        endlessWave++;
        hp = lastHp;
        allowSpawn = true;
        time = PlayerPrefs.GetInt(Options.speedKey, 2);
        switch (time)
        {
            case 0:
                Time.timeScale = 0.5f;
                break;
            case 1:
                Time.timeScale = 0.75f;
                break;
            case 2:
                Time.timeScale = 1;
                break;
            case 3:
                Time.timeScale = 1.5f;
                break;
            case 4:
                Time.timeScale = 2;
                break;
        }
        inMenu = !(SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2);
    }

    // Update is called once per frame
    void Update()
    {
        print("CurrentDMG:" + increaseDMG);
        print("CurrentBoomerDMG:" + increaseBDMG);
        //print("Whats the time?" + Time.timeScale);
        print("Current EndlessWave:" + endlessWave);
        if (inMenu)
        {
            endlessWave = 0;
            playerHealth.enabled = false;
            wave.enabled = false;
            playerHealthText.enabled = false;
            waveText.enabled = false;
            bossHealth.enabled = false;
            bossUI.enabled = false;
            cooldownText.enabled = false;
            cooldownNumber.enabled = false;
            ui.SetActive(false);
            return;
        } 
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            print("I");
            //Instantiate(enemys[0], new Vector3(10f,Random.Range(-5f, 1f),0), Quaternion.identity);
            waveNumber = 16;
            hp = 99999;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            lastWaveNumber = waveNumber;
            //lastHp = hp;
            SceneManager.LoadScene(3);
        }

        if (allowSpawn)
        {
            currentDiff = PlayerPrefs.GetInt(Options.diffKey, 0);
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
                if (currentDiff > 0)
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

                if (currentDiff == 2)
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
                if (currentDiff > 0)
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

                if (currentDiff == 2)
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
                if (currentDiff > 0)
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

                if (currentDiff == 2)
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
                if (currentDiff > 0)
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

                if (currentDiff == 2)
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
            }
            //GameObject cat2 = Instantiate(enemys[0], new Vector3(Random.Range(10f, 15f),Random.Range(-5f, 1f),0), Quaternion.identity);
            //GameObject cat4 = Instantiate(enemys[0], new Vector3(Random.Range(10f, 15f),Random.Range(-5f, 1f),0), Quaternion.identity);
            //GameObject cat8 = Instantiate(enemys[0], new Vector3(Random.Range(10f, 15f),Random.Range(-5f, 1f),0), Quaternion.identity);
            //GameObject catBoss = Instantiate(enemys[0], new Vector3(Random.Range(10f, 15f),Random.Range(-5f, 1f),0), Quaternion.identity);


        }

        allowSpawn = false;
        
        //print(activeEnemies);
        if (hp > -1)
        {
            playerHealth.text = hp.ToString();
        }
        wave.text = waveNumber.ToString();
        //print($"Die Werte: Allow Spawn?: {allowSpawn} Boss spawn? {spawnBoss}");

        if (activeEnemies == 0 && !endOfWave)
        {
            if (waveNumber != 5 && waveNumber != 10 && waveNumber != 15)
            {
                allowSpawn = true;
            }
            if (waveNumber < 16)
            {
                spawnBoss = false;
                lastWaveNumber = waveNumber;
                waveNumber++;
                
                //print("Incerased Wave");
            }
            endOfWave = true;
        }

        if (boss)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                bossMaxHP = 300;
            }
            else
            {
                bossMaxHP = 300 * endlessWave;
            }
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
            if (bossHP > -1)
            {
                bossHealth.text = bossHP.ToString();
            }
            spawnBoss = true;
        }

        if (bossLevel)
        {
            //print("Did I spawn?");
            Instantiate(enemys[4], new Vector3(15f, 0, 0), quaternion.identity);
            spawnBoss = false;
            bossLevel = false;
            boss = true;
        }
        if (activeEnemies > 0)
        {
            endOfWave = false;
        }
        if (hp <= 0)
        {
            hp = 0;
            print("GAME OVER");
            gameOver.SetActive(true);
            Time.timeScale = 0;
            //SceneManager.LoadScene(0);
        }

        if (bossHP <= 0 && bossCanDie)
        {
            bossCanDie = false;
            bossHP = 0;
            waveNumber = 0;
            //bossHP = 30;
            Time.timeScale = 0;

            if (!endlessMode)
            {
                print("EndlessMode UNLOCKED");
                endlessMode = true;
                PlayerPrefs.SetInt(endlessModeKey, endlessMode ? 1 : 0);
                SceneManager.LoadScene(5);
            } 
            else if (endlessMode)
            {
                if (!isOpen)
                {
                    upgradeScreen.OpenScreen();
                    isOpen = true;
                }
            }
        }

        if (!bossCanDie)
        {
            if (bossHP <= 0 || bossReachedMaxHealth)
            {
                bossReachedMaxHealth = true;
                if (bossHPCooldown > bossHPCounter)
                {
                    // if (bossHP % 100 == 99) bossHP += 2;
                    // else bossHP++;
                    
                    bossHP++;
                    
                    bossHPCooldown = 0;
                    bossHPCounter -= 1f;
                }
                bossHPCooldown += Time.deltaTime;
            }
        }

        if (bossHP >= bossMaxHP)
        {
            bossCanDie = true;
            bossReachedMaxHealth = false;

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
        hp -= endlessWave;
    }

    public void ActiveEnemiesAdd()
    {
        activeEnemies++;
    }
    
    public void ActiveEnemiesRemove()
    {
        activeEnemies--;
        hp += healthPerKill;
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

    public void BossGotHit(int playerStrength)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            playerStrength = 1;
        }
        bossHP -= playerStrength;
    }

    public void BossPhase()
    {
        //Fake Death 30 -> 300
        //Spawn Bombcats, Toxicball, Enemy Wave Spawn

        if (bossCanDie)
        {
            if (bossHP <= bossMaxHP / 3)
            {
                //Toxicball
                if (BossAllowSpawn)
                {
                    //print("???");
                    Instantiate(toxicBall, new Vector3(15f, Random.Range(1, -4), 0), Quaternion.identity);
                    Instantiate(toxicBall, new Vector3(15f, Random.Range(1, -4), 0), Quaternion.identity);
                }
            }
            if (bossHP <= bossMaxHP / 2)
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
            if (bossHP <= bossMaxHP)
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

    public int GetBossHP => bossHP;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Level 2"))
        {
            print("Lets switch to Level 2!");
            uiBoomer.SetActive(true);
            allowSpawn = true;
        }
        if (collision.CompareTag("Level 3"))
        {
            print("Lets switch to Level 3!");
            allowSpawn = true;
        }
        if (collision.CompareTag("Level Boss"))
        {
            print("Lets switch to Level Boss!");
            bossHP = 30;
            uiBoss.SetActive(true);
            bossUI.enabled = true;
            bossHealth.enabled = true;
            bossLevel = true;
            allowSpawn = true;
        }
    }

    public void Armor(int armor)
    {
        hp += armor;
    }

    public void PowerUps(Upgrade.PowerUpType item)
    {
        switch (item)
        {
            case Upgrade.PowerUpType.Attack:
                //it works
                increaseDMG++;
                break;
            case Upgrade.PowerUpType.Health:
                //it works
                hp += 5 * endlessWave;
                break;
            case Upgrade.PowerUpType.BCooldown:
                resetBCD = true;
                break;
            case Upgrade.PowerUpType.BDamage:
                //it works
                increaseBDMG++;
                break;
            case Upgrade.PowerUpType.Regeneration:
                //it works
                healthPerKill++;    
                break;
            // case Upgrade.PowerUpType.Points:
            //     hp += 5 * endlessWave;
            //     break;
        }
    }

    public void SaveHP()
    {
        lastHp = hp;
    }

    public int GetEndlessNumber()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            return 1;
        }
        // if (increaseBDMG)
        // {
        //     increaseBDMG = false;
        //     return endlessWave++;
        // }
        if (resetBCD)
        {
            resetBCD = false;
            return 1;
        }
        return endlessWave;
    }

    public int GetBDMG()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            return 1;
        }
        return increaseBDMG;
    }
    public int GetDMG()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            return 1;
        }
        return increaseDMG;
    }
}
