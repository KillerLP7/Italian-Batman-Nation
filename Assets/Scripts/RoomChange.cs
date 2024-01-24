using UnityEngine;

public class RoomChange : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]private bool block;
    public AudioSource src;
    private int currentWave;

    private void Awake()
    {
        if (!block)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        src.volume = PlayerPrefs.GetFloat(Options.audioKey, 1) / 2;
        if (!block)
        {
            rb.velocity = new Vector2(-1, 0);
        }
        //-17.8
        //-35,6
        //-53,4
        if (block)
        {
            currentWave = GameManager.Instance.GetWaveNumber();
            if (currentWave == 6)
            {
                transform.position = new Vector3(17.8f, 10f, 0);
            }

            if (currentWave == 11)
            {
                transform.position = new Vector3(0f, 10f, 0);
            }

            if (currentWave == 16)
            {
                transform.position = new Vector3(-17.8f, 10f, 0);
            }
        }
    }
}
