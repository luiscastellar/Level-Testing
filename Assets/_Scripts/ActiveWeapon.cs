using System;
using Cinemachine;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] WeaponSO _weaponSO;
    [SerializeField] CinemachineVirtualCamera playerFollowCamera;
    [SerializeField] GameObject zoomVignette;
    
    Animator _animator;
    StarterAssetsInputs _starterAssetsInputs;
    FirstPersonController _firstPersonController;
    Weapon _currentWeapon;

    const string SHOOT_STRING = "Shoot";

    float _timeSinceLastShot = 0f;
    float defaultFOV;
    float defaultRotationSpeed;
    
    void Awake()
    {
        _starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        _firstPersonController = GetComponentInParent<FirstPersonController>();
        _animator = GetComponent<Animator>();
        defaultFOV = playerFollowCamera.m_Lens.FieldOfView;
        defaultRotationSpeed = _firstPersonController.RotationSpeed;
        zoomVignette.SetActive(false);
    }

    void Start()
    {
        _currentWeapon = GetComponentInChildren<Weapon>();
    }

    void Update()
    {
        HandleShoot();
        HandleZoom();
    }
    
    public void SwitchWeapon(WeaponSO weaponSo)
    {
        if(_currentWeapon)
        {
            Destroy(_currentWeapon.gameObject);
        }
        
        Weapon newWeapon = Instantiate(weaponSo.WeaponPrefab, transform).GetComponent<Weapon>();
        _currentWeapon = newWeapon;
        _weaponSO = weaponSo;
    }

    void HandleShoot()
    {
        _timeSinceLastShot += Time.deltaTime;
        
        if (!_starterAssetsInputs.shoot) return;
        
        if (_timeSinceLastShot >= _weaponSO.FireRate)
        {
            _currentWeapon.Shoot(_weaponSO);
            _animator.Play(SHOOT_STRING, 0, 0f);
            _timeSinceLastShot = 0f;
        }

        if (!_weaponSO.IsAutomatic)
        {
            _starterAssetsInputs.ShootInput(false);
        }
    }

    void HandleZoom()
    {
        if(!_weaponSO.CanZoom) return;

        if (_starterAssetsInputs.zoom)
        {
            playerFollowCamera.m_Lens.FieldOfView = _weaponSO.ZoomAmount;
            zoomVignette.SetActive(true);
            _firstPersonController.ChangeRotationSpeed(_weaponSO.ZoomRotationSpeed);
        }
        else
        {
            playerFollowCamera.m_Lens.FieldOfView = defaultFOV;
            zoomVignette.SetActive(false);
            _firstPersonController.ChangeRotationSpeed(defaultRotationSpeed);
        }
    }
}
