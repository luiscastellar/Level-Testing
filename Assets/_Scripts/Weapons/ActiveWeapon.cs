using System;
using Cinemachine;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] WeaponSO startWeapon;
    [SerializeField] CinemachineVirtualCamera playerFollowCamera;
    [SerializeField] Camera weaponCamera;
    [SerializeField] GameObject zoomVignette;
    [SerializeField] TMP_Text ammoText;
    
    Animator _animator;
    StarterAssetsInputs _starterAssetsInputs;
    FirstPersonController _firstPersonController;
    Weapon _currentWeapon;
    WeaponSO _currentWeaponSo;
    
    const string SHOOT_STRING = "Shoot";

    float _timeSinceLastShot;
    float _defaultFOV;
    float _defaultRotationSpeed;

    private int _currentAmmo = 0;
    
    void Awake()
    {
        _starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        _firstPersonController = GetComponentInParent<FirstPersonController>();
        _animator = GetComponent<Animator>();
        _defaultFOV = playerFollowCamera.m_Lens.FieldOfView;
        _defaultRotationSpeed = _firstPersonController.RotationSpeed;
        zoomVignette.SetActive(false);
    }

    void Start()
    {
        SwitchWeapon(startWeapon);
        AdjustAmmo(_currentWeaponSo.MagazineSize);
    }

    void Update()
    {
        HandleShoot();
        HandleZoom();
    }

    public void AdjustAmmo(int amount)
    {
        _currentAmmo += amount;

        if (_currentAmmo > _currentWeaponSo.MagazineSize)
        {
            _currentAmmo = _currentWeaponSo.MagazineSize;
        }
        ammoText.text = _currentAmmo.ToString("D2");
    }
    
    public void SwitchWeapon(WeaponSO weaponSo)
    {
        if(_currentWeapon)
        {
            Destroy(_currentWeapon.gameObject);
        }
        
        Weapon newWeapon = Instantiate(weaponSo.WeaponPrefab, transform).GetComponent<Weapon>();
        _currentWeapon = newWeapon;
        _currentWeaponSo = weaponSo;
        
        AdjustAmmo(_currentWeaponSo.MagazineSize); 
    }

    void HandleShoot()
    {
        _timeSinceLastShot += Time.deltaTime;
        
        if (!_starterAssetsInputs.shoot) return;

        if (_timeSinceLastShot >= _currentWeaponSo.FireRate && _currentAmmo > 0)
        {
            _currentWeapon.Shoot(_currentWeaponSo);
            _animator.Play(SHOOT_STRING, 0, 0f);
            _timeSinceLastShot = 0f;
            AdjustAmmo(-1);
        }

        if (!_currentWeaponSo.IsAutomatic)
        {
            _starterAssetsInputs.ShootInput(false);
        }
    }

    void HandleZoom()
    {
        if(!_currentWeaponSo.CanZoom) return;

        if (_starterAssetsInputs.zoom)
        {
            playerFollowCamera.m_Lens.FieldOfView = _currentWeaponSo.ZoomAmount;
            weaponCamera.fieldOfView = _currentWeaponSo.ZoomAmount;
            zoomVignette.SetActive(true);
            _firstPersonController.ChangeRotationSpeed(_currentWeaponSo.ZoomRotationSpeed);
        }
        else
        {
            playerFollowCamera.m_Lens.FieldOfView = _defaultFOV;
            weaponCamera.fieldOfView = _defaultFOV;
            zoomVignette.SetActive(false);
            _firstPersonController.ChangeRotationSpeed(_defaultRotationSpeed);
        }
    }
}
