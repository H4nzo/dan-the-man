using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [Header("Fire")]
    [SerializeField] float fireRange;
    [SerializeField] float fireRate;//rounds per second
    float timeSinceLastFire;

    [SerializeField][Range(0, 10)] int FireScatter;
    [SerializeField] Transform firePoint;
    [SerializeField] Rigidbody2D projectile;

    public override void Patrol()
    {
        base.Patrol();
        if (MoveTo(patrolTarget) == true)
        {
            ChooseTarget();
            idleTime = 1.5f;
        }
    }

    public override void Attack()
    {
        base.Attack();
        timeSinceLastFire += Time.deltaTime;
        if (!InFireRange(target))
        {
            MoveTo(target);
        }
        else
        {
            Fire();
        }
    }

    bool InFireRange(Transform t)
    {
        float dist = Mathf.Abs(t.position.x - transform.position.x);
        if (dist < fireRange && Mathf.Sign(transform.right.x) == Mathf.Sign((t.position - transform.position).x))
        {
            return true;
        }
        return false;
    }

    void Fire()
    {
        StopWalking();
        if (timeSinceLastFire > 1.0 / fireRate)
        {
            //
            //
            timeSinceLastFire = 0;
            //
            //
            var bullet = Instantiate(projectile, firePoint.position, projectile.transform.rotation);
            bullet.velocity = firePoint.right * 50;
            //
            //
            int numberOfBullets = FireScatter / 2;
            //
            //
            if (numberOfBullets <= 0) return;
            for (int i = 0; i < numberOfBullets; i++)
            {
                float angle = (2 * (i + 1));
                //
                bullet = Instantiate(projectile, firePoint.position, Quaternion.AngleAxis(angle, transform.forward) * projectile.transform.rotation);
                bullet.velocity = Quaternion.AngleAxis(angle, transform.forward) * firePoint.right * 50;
                //
                bullet = Instantiate(projectile, firePoint.position, Quaternion.AngleAxis(-angle, transform.forward) * projectile.transform.rotation);
                bullet.velocity = Quaternion.AngleAxis(-angle, transform.forward) * firePoint.right * 50;
            }
        }
    }

    private new void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.right * fireRange);
    }
}
