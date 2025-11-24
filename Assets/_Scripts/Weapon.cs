using System;
using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem hitVFXPrefab;
    [SerializeField] int damageAmount = 1;
    
    StarterAssetsInputs _starterAssetsInputs;

    private const string SHOOT_STRING = "Shoot";
    
    private void Awake()
    {
        _starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
    }

    void Update()
    {
        HandleShoot();
    }

    void HandleShoot()
    {
        if (!_starterAssetsInputs.shoot) return;
        
        muzzleFlash.Play();
        animator.Play(SHOOT_STRING, 0, 0f);
        
        _starterAssetsInputs.ShootInput(false);
        
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {   
            Instantiate(hitVFXPrefab, hit.point, Quaternion.identity);
            
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            enemyHealth?.TakeDamage(damageAmount);
        }
    }
}
