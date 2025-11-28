using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int startingHealth = 3;
    [SerializeField] GameObject explosionVFX;
    
    int _currentHealth;

    void Awake()
    {
        _currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        
        if (_currentHealth <= 0)
        {
            SelfDestruct();
        }
    }

    public void SelfDestruct()
    {
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
