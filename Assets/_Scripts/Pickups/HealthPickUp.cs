using System;
using UnityEngine;

public class HealthPickUp : Pickup
{
    [SerializeField] int healthCuantity;

    protected override void OnPickup(ActiveWeapon activeWeapon, PlayerHealth playerHealth)
    {
        playerHealth.Heal(healthCuantity);
    }
}
