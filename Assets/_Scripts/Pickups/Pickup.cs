using System;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    const string PLAYER_STRING = "Player";
    
    [SerializeField] AudioClip pickupSound;

    void Start()
    {
        //PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_STRING))
        {
            ActiveWeapon activeWeapon = other.GetComponentInChildren<ActiveWeapon>();
            PlayerHealth.PlaySound(pickupSound);
            OnPickup(activeWeapon);
            Destroy(gameObject);
        }
    }
    
    protected abstract void OnPickup(ActiveWeapon activeWeapon);
}
