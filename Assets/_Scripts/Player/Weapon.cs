using System;
using Cinemachine;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] LayerMask interactionLayers;
    
    CinemachineImpulseSource _impulseSource;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shoot(WeaponSO weaponSO)
    {
        muzzleFlash.Play();
        _impulseSource.GenerateImpulse();

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out var hit, Mathf.Infinity, interactionLayers, QueryTriggerInteraction.Ignore))
        {   
            Instantiate(weaponSO.HitVFXPrefab, hit.point, Quaternion.identity);
            EnemyHealth enemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();
            enemyHealth?.TakeDamage(weaponSO.Damage);
        }
    }
}
