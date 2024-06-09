using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var idamageable = collision.gameObject.GetComponent<IDamageable>();
        idamageable?.TakeDamage(10);
        Destroy(gameObject);
    }
}
