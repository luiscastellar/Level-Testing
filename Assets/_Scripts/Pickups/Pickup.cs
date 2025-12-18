using System;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    const string PLAYER_STRING = "Player";
    
    [SerializeField] AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_STRING))
        {
            ActiveWeapon activeWeapon = other.GetComponentInChildren<ActiveWeapon>();
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            PlayerHealth.PlaySound(pickupSound);
            OnPickup(activeWeapon, playerHealth);
            Destroy(gameObject);
        }
    }
    
    protected abstract void OnPickup(ActiveWeapon activeWeapon, PlayerHealth playerHealth);
}
