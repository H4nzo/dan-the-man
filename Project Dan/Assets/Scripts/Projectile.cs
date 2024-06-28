using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float start = 0;
    void Start()
    {
        start = Time.time;
    }

    void Update()
    {
        if(Time.time - start > 15)
        {
            Dump();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Projectile")) return;

        var idamageable = collision.gameObject.GetComponent<IDamageable>();
        idamageable?.TakeDamage(10);
        
        Dump();

        Debug.Log($"I just collided with {collision.transform.name}");
    }

    void Dump()
    {
        gameObject.SetActive(false);
        Trash.Dump(gameObject);
    }
}
