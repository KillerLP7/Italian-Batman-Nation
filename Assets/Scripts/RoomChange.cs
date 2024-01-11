using UnityEngine;

public class RoomChange : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]private bool block;

    private void Awake()
    {
        if (!block)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        if (!block)
        {
            rb.velocity = new Vector2(-1, 0);
        }
        //-17.8
        //-35,6
        //-53,4
    }
}
