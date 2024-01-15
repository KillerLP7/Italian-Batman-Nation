using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float counter;
    // Update is called once per frame
    void Update()
    {
        
        
        if (counter > 0.1)
        {
            Destroy(gameObject);
            counter = 0;
        }
        counter += Time.deltaTime;
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (collision.gameObject.CompareTag("Boss"))
        {
            print("Hit!");
        }
        
    }
}
