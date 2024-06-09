using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    [SerializeField] Throwable knifeObject;

    void Start()
    {

    }

    void Update()
    {

    }

    public override void Fire()
    {
        if (ammoCount <= 0) return;
        base.Fire();
        var knife = Instantiate(knifeObject, transform.position, transform.rotation);
    }
}
