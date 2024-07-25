using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int ammoCount;
    public Sprite icon;
    public WeaponType weaponType; // Add this field

    void Start()
    {

    }

    void Update()
    {

    }

    public virtual void Fire()
    {

    }
}
