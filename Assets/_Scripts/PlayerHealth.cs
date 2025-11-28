using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] int startingHealth = 5;
    [SerializeField] CinemachineVirtualCamera deathVirtualCamera;
    [SerializeField] Transform weaponCamera;
    [SerializeField] Image[] shieldBars;
    
    int _currentHealth;
    int _gameOverVirtualCameraPriority = 20;

    void Awake()
    {
        _currentHealth = startingHealth;
        
        AdjustShieldUI();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        AdjustShieldUI();
        
        if (_currentHealth <= 0)
        {
            weaponCamera.parent = null;
            deathVirtualCamera.Priority = _gameOverVirtualCameraPriority;
            Destroy(gameObject);
        }
    }

    void AdjustShieldUI()
    {
        for (int i = 0; i < shieldBars.Length; i++)
        {
            shieldBars[i].gameObject.SetActive(i < _currentHealth);
        }
    }
}
