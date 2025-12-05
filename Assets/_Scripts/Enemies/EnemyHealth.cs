using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int startingHealth = 3;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] AudioClip explodeSound;
    [SerializeField] EnemyHealthBar healthBarPrefab;
    [SerializeField] Transform healthBarPoint;
    
    int _currentHealth;
    EnemyHealthBar _healthBar;
    
    GameManager _gameManager;

    void Awake()
    {
        _currentHealth = startingHealth;
    }

    private void Start()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        _gameManager.AdjustEnemiesLeft(1);
        
        _healthBar = Instantiate(
            healthBarPrefab, 
            healthBarPoint.position, 
            Quaternion.identity,
            transform
        );
        
        _healthBar.SetMaxHealth(startingHealth);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _healthBar.SetHealth(_currentHealth);
        
        if (_currentHealth <= 0)
        {
            SelfDestruct();
        }
    }

    public void SelfDestruct()
    {
        PlayerHealth.PlaySound(explodeSound);
        _gameManager.AdjustEnemiesLeft(-1);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
