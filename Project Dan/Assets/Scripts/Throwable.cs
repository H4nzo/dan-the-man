using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 dir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0;
        rb.angularDrag = 0;
        rb.drag = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        dir = transform.right;
    }

    void Update()
    {
        rb.velocity = dir * 15;
        rb.angularVelocity = -transform.forward.z * 360 * 5f;
    }
}