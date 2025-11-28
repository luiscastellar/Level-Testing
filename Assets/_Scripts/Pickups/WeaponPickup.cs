using System;
using UnityEngine;

public class WeaponPickup : Pickup
{
    [SerializeField] private WeaponSO _weaponSo;
    
    protected override void OnPickup(ActiveWeapon activeWeapon)
    {
        activeWeapon.SwitchWeapon(_weaponSo);
    }
}