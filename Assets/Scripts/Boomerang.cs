using System;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [SerializeField] private GameObject attack;
    private Rigidbody2D rb;
    private Vector3 playerPos;
    private bool getBack;
    private float counter;
    private float speed = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerPos = GameManager.Instance.GetPlayerPos(true);

        rb.velocity = Vector2.right * ( playerPos.x < transform.position.x ? speed : -speed);
    }

    // Update is called once per frame
    private void Update()
    {
        playerPos = GameManager.Instance.GetPlayerPos(true);
        
        if (transform.position.x is > 8 or < -8) getBack = true;

        if (getBack)
        {
            //rb.velocity = (playerPos - transform.position).normalized * 10;
            if (playerPos.x > transform.position.x)
            {
                rb.velocity = new Vector2(2, 0);
            }
            if (playerPos.x < transform.position.x)
            {
                rb.velocity = new Vector2(-2, 0);
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
            }
        }
        
        if (counter > 0.2)
        {
            Instantiate(attack, transform.position, Quaternion.identity);
            counter = 0;
        }
        
        counter += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) Destroy(gameObject);
    }
}
