using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera deathVirtualCamera;
    [SerializeField] Transform weaponCamera;
    [SerializeField] Image[] shieldBars;
    [SerializeField] GameObject gameOverContainer;
    [SerializeField] DamageFeedbackUI damageFeedbackUI;
    [SerializeField] CameraShake cameraShake;
    [SerializeField] AudioClip hitSound;
    
    [Range(1, 10)]
    [SerializeField] int startingHealth = 8;
    
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
        BattlePressureManager.Instance?.ModifyPressure(8f);
        damageFeedbackUI?.PlayDamageFlash();
        cameraShake?.Shake(Mathf.Clamp(damage * 0.4f, 0.5f, 2f), 0.15f);
        _audioSource.PlayOneShot(hitSound);
        
        _currentHealth -= damage;

        AdjustShieldUI();
        
        if (_currentHealth <= 0)
        {
            PlayerGameOver();
        }
    }
    
    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, startingHealth);
        AdjustShieldUI();
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
