using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] Transform shootPoint;
    SpriteRenderer spriteRenderer;

    [SerializeField] GameObject muzzleFlash;

    [SerializeField] Rigidbody2D projectile;

    List<GameObject> destructibles;
    float destructTimer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Disable();
    }

    void Update()
    {

    }

    public void Enable()
    {
        spriteRenderer.enabled = true;
    }

    public void Disable()
    {
        spriteRenderer.enabled = false;
    }

    public override void Fire()
    {
        if (ammoCount <= 0) return;
        base.Fire();
        StopAllCoroutines();
        StartCoroutine(fireRoutine());
    }

    void Shoot()
    {
        var flash = Instantiate(muzzleFlash, shootPoint.position, shootPoint.rotation);
        var bullet = Instantiate(projectile, shootPoint.position, Quaternion.identity);
        bullet.velocity = shootPoint.right * 50;
    }

    IEnumerator fireRoutine()
    {
        //Spawn Gun
        Enable();
        //Spawn Gun
        yield return null;
        //Shoot
        Shoot();
        //Shoot
        yield return new WaitForSeconds(0.4f);
        //Despawn Gun
        Disable();
    }
}
