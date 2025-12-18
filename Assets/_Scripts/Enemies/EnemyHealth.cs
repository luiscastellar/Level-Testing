using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int pickUpSpawnProbability = 5;
    [SerializeField] int startingHealth = 3;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] GameObject[] pickUps;
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
            PickUpInstance();
        }
    }

    private void PickUpInstance()
    {
        int random = UnityEngine.Random.Range(0, pickUpSpawnProbability);
        if (random == 0)
        {
            int index = UnityEngine.Random.Range(0, pickUps.Length);
            Instantiate(pickUps[index], transform.position, Quaternion.identity);
        }

        SelfDestruct();
    }

    public void SelfDestruct()
    {
        BattlePressureManager.Instance?.ModifyPressure(-10f);
        PlayerHealth.PlaySound(explodeSound);
        _gameManager.AdjustEnemiesLeft(-1);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
