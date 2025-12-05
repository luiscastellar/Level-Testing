using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] int startingHealth = 5;
    [SerializeField] CinemachineVirtualCamera deathVirtualCamera;
    [SerializeField] Transform weaponCamera;
    [SerializeField] Image[] shieldBars;
    [SerializeField] GameObject gameOverContainer;
    
    static AudioSource _audioSource;
    
    int _currentHealth;
    int _gameOverVirtualCameraPriority = 20;

    void Awake()
    {
        _currentHealth = startingHealth;
        _audioSource = GetComponent<AudioSource>();
        AdjustShieldUI();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        AdjustShieldUI();
        
        if (_currentHealth <= 0)
        {
            PlayerGameOver();
        }
    }

    void PlayerGameOver()
    {
        weaponCamera.parent = null;
        deathVirtualCamera.Priority = _gameOverVirtualCameraPriority;
        gameOverContainer.SetActive(true);
        StarterAssetsInputs starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
        starterAssetsInputs.SetCursorState(false);
        Destroy(gameObject);
    }

    void AdjustShieldUI()
    {
        for (int i = 0; i < shieldBars.Length; i++)
        {
            shieldBars[i].gameObject.SetActive(i < _currentHealth);
        }
    }

    public static void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
