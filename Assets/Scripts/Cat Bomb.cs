using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBomb : MonoBehaviour
{
    [SerializeField] private GameObject attack;
    private float counter;

    private float attackCooldown;
    // Update is called once per frame
    void Update()
    {
        
        
        if (counter > 15)
        {
            Destroy(gameObject);
            counter = 0;
        }
        counter += Time.deltaTime;
        if (attackCooldown > 0.5)
        {
            Instantiate(attack, transform.position, Quaternion.identity);
            attackCooldown = 0;
        }
        attackCooldown += Time.deltaTime;
        
    }
}
