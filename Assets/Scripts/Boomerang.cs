using UnityEngine;
using UnityEngine.Serialization;

public class Boomerang : MonoBehaviour
{
    [FormerlySerializedAs("Playerattack")] [FormerlySerializedAs("attack")] [SerializeField] private GameObject playerAttack;
    [SerializeField] private GameObject enemyAttack;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector3 playerPos;
    private bool getBack;
    private float counter;
    private float speed = 5f;
    [SerializeField] private bool gun;
    private bool activate;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerPos = GameManager.Instance.GetPlayerPos(true);

        if (!gun)
        {
            rb.velocity = Vector2.right * ( playerPos.x < transform.position.x ? speed : -speed);
        }

        activate = true;
    }

    // Update is called once per frame
    private void Update()
    {
        playerPos = GameManager.Instance.GetPlayerPos(true);

        if (!gun)
        {
            if (transform.position.x is > 8 or < -8) getBack = true;

            if (getBack)
            {
                rb.velocity = (playerPos - transform.position).normalized * 10;
                /*if (playerPos.x > transform.position.x)
            {
                rb.velocity = new Vector2(3, 0);
            }
            if (playerPos.x < transform.position.x)
            {
                rb.velocity = new Vector2(-3, 0);
            }

            if (Math.Abs(playerPos.x - transform.position.x) < 0.2f)
            {
                if (playerPos.y > transform.position.y)
                {
                    rb.velocity = new Vector2(0, 10);
                }
                if (playerPos.y < transform.position.y)
                {
                    rb.velocity = new Vector2(0, -10);
                }
            }*/
            }
        }

        if (gun)
        {
            if (transform.position.x < 0 && activate)
            {
                sr.flipX = true;
                rb.velocity = new Vector2(5f, 0);
                activate = false;
            }
        
            if (transform.position.x > 0 && activate)
            {   sr.flipX = false;
                rb.velocity = new Vector2(-5f, 0);
                activate = false;
            }
        }
        
        if (counter > 0.1)
        {

            if (!gun)
            {
                Instantiate(playerAttack, transform.position, Quaternion.identity);
            }

            if (gun)
            {
                Instantiate(enemyAttack, transform.position, Quaternion.identity);
            }
            counter = 0;
        }
        
        counter += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gun)
            {
                GameManager.Instance.PlayerGotHit();
            }
            Destroy(gameObject);
        }

        if (collision.CompareTag("Destroy"))
        {
            Destroy(gameObject);
        }
        
    }
}
