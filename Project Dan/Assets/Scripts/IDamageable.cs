using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public int Health { get; set; }
    public bool Dead { get; set; }
    public void TakeDamage(int damage);
    //public void Die();
}
