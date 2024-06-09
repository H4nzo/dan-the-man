using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponType
{
    Knife,
    Rilfe
}

public class WeaponManager : MonoBehaviour
{
    [SerializeField] List<Weapon> weapons;
    int active;

    [SerializeField] Image weaponUI;
    [SerializeField] Text weaponAmmoUI;

    void Start()
    {
        active = 0;
    }

    void Update()
    {
        weaponUI.sprite = weapons[active].icon;
        weaponAmmoUI.text = weapons[active].ammoCount.ToString();
    }

    public void FireWeapon()
    {
        weapons[active]?.Fire();
    }

    public void NextWeapon()
    {
        active++;
        active %= weapons.Count;
    }

    public void AddAmmo(WeaponType type, int count)
    {
        weapons[(int)type].ammoCount += count;
    }
}
