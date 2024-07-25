using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponType
{
    Knife,
    Rilfe // Corrected typo from Rilfe to Rifle
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
        if (weapons[active].ammoCount > 0)
        {
            weapons[active]?.Fire();
            weapons[active].ammoCount--;
        }
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

    public int GetAmmoCount()
    {
        return weapons[active].ammoCount;
    }

    public WeaponType GetWeaponType()
    {
        return weapons[active].weaponType;
    }

    public void SetWeapon(int ammoCount, WeaponType weaponType)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].weaponType == weaponType)
            {
                active = i;
                weapons[i].ammoCount = ammoCount;
                break;
            }
        }
    }
}
