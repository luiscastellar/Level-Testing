using System;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponSO _weaponSo;
    
    const string PLAYER_STRING = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_STRING))
        {
            ActiveWeapon activeWeapon = other.gameObject.GetComponentInChildren<ActiveWeapon>();
            activeWeapon.SwitchWeapon(_weaponSo);
            Destroy(gameObject);
        }
    }
}
