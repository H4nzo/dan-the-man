using System.Collections;
using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    float attackDelay;
    bool attacking;
    
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
        if (!InMeleeRange(target) && !attacking)
        {
            MoveTo(target);
        }
        else
        {
            MeleeAttack();
        }
    }

    bool InMeleeRange(Transform t)
    {
        float dist = Mathf.Abs(t.position.x - transform.position.x);
        if (dist < 1.5f && Mathf.Sign(transform.right.x) == Mathf.Sign((t.position - transform.position).x))
        {
            return true;
        }
        return false;
    }

    void MeleeAttack()
    {
        StopWalking();
        if (attackDelay > 0) { attackDelay -= Time.deltaTime; return; }

        animator.Play("Attack", 0, 0);
        AttackLogic();

        attackDelay = 1;
    }

    public void AttackLogic()
    {
        attacking = true;
        if (Vector3.Distance(transform.position, target.position) <= 2.3f)
        {
            target.GetComponent<Rigidbody2D>().AddForce((target.position - transform.position).normalized * 20, ForceMode2D.Impulse);
            target.GetComponent<IDamageable>()?.TakeDamage(-DamageLevel);
        }
        
        Invoke("stopAttack", 0.7f);
    }
    void stopAttack()
    {
        attacking = false;
    }
}
