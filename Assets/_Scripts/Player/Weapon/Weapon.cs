using System;
using Cinemachine;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] LayerMask interactionLayers;

    public WeaponSO WeaponSo { get; private set; }
    public int CurrentAmmo;

    CinemachineImpulseSource _impulseSource;
    Camera _camera;

    void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _camera = Camera.main;
    }

    public void Setup(WeaponSO weaponSO)
    {
        WeaponSo = weaponSO;
    }

    public void Shoot()
    {
        if (WeaponSo == null) return;
        if (CurrentAmmo <= 0) return;

        muzzleFlash.Play();
        if (_impulseSource) _impulseSource.GenerateImpulse();

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward,
                out var hit, Mathf.Infinity, interactionLayers, QueryTriggerInteraction.Ignore))
        {
            Instantiate(WeaponSo.HitVFXPrefab, hit.point, Quaternion.identity);
            EnemyHealth enemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();
            enemyHealth?.TakeDamage(WeaponSo.Damage);
        }

        CurrentAmmo--;
    }
}
