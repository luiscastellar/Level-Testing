using System;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponPickup : Pickup
{
    [SerializeField] WeaponSO _weaponSO;
    
    protected override void OnPickup(ActiveWeapon activeWeapon, PlayerHealth playerHealth)
    {
        activeWeapon.AddWeapon(_weaponSO, giveFullMagazine: true);
    }
}