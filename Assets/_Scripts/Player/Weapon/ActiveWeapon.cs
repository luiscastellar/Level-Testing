using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] WeaponSO startWeapon;
    [SerializeField] CinemachineVirtualCamera playerFollowCamera;
    [SerializeField] Camera weaponCamera;
    [SerializeField] GameObject zoomVignette;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] WeaponHUD weaponHUD;
    
    Animator _animator;
    StarterAssetsInputs _starterAssetsInputs;
    FirstPersonController _firstPersonController;
    AudioSource _audioSource;

    List<Weapon> _weapons = new List<Weapon>();
    int _currentWeaponIndex = 0;

    Weapon _currentWeapon;
    WeaponSO _currentWeaponSo;

    float _timeSinceLastShot;
    float _defaultFOV;
    float _defaultRotationSpeed;

    const string SHOOT_STRING = "Shoot";

    [SerializeField] float switchWeaponCooldown = 0.2f;
    float _lastSwitchTime = -10f;

    void Awake()
    {
        _starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        _firstPersonController = GetComponentInParent<FirstPersonController>();
        _animator = GetComponent<Animator>();

        _defaultFOV = playerFollowCamera.m_Lens.FieldOfView;
        _defaultRotationSpeed = _firstPersonController.RotationSpeed;

        zoomVignette.SetActive(false);

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    void Start()
    {
        if (startWeapon != null)
            AddWeapon(startWeapon, giveFullMagazine: true);

        if (_weapons.Count > 0)
            EquipWeapon(0);
    }

    void Update()
    {
        HandleShoot();
        HandleZoom();
        HandleWeaponSwitch();
    }

    // ===================== INVENTARIO =======================

    public void AddWeapon(WeaponSO weaponSo, bool giveFullMagazine = false)
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            if (_weapons[i].WeaponSo == weaponSo)
            {
                int add = weaponSo.MagazineSize;
                _weapons[i].CurrentAmmo += add;
                if (_weapons[i].CurrentAmmo > weaponSo.MagazineSize)
                    _weapons[i].CurrentAmmo = weaponSo.MagazineSize;
                
                if (i == _currentWeaponIndex)
                    UpdateAmmoText(_weapons[i].CurrentAmmo, weaponSo.MagazineSize);

                return;
            }
        }

        Weapon newWeapon = Instantiate(weaponSo.WeaponPrefab, transform).GetComponent<Weapon>();
        newWeapon.Setup(weaponSo);
        newWeapon.gameObject.SetActive(false);
        newWeapon.CurrentAmmo = giveFullMagazine ? weaponSo.MagazineSize : 0;

        _weapons.Add(newWeapon);
        
        weaponHUD.AddWeapon(weaponSo);
    }

    void EquipWeapon(int index)
    {
        if (_weapons.Count == 0) return;
        if (index < 0 || index >= _weapons.Count) return;

        if (_currentWeapon)
            _currentWeapon.gameObject.SetActive(false);

        _currentWeaponIndex = index;
        _currentWeapon = _weapons[index];
        _currentWeapon.gameObject.SetActive(true);
        _currentWeaponSo = _currentWeapon.WeaponSo;

        int curAmmo = _currentWeapon.CurrentAmmo;
        
        UpdateAmmoText(curAmmo, _currentWeaponSo.MagazineSize);
        weaponHUD.UpdateSelection(_currentWeaponIndex);
    }
    
    public void GiveAmmoToCurrentWeapon(int amount)
    {
        if (_currentWeapon == null || _currentWeaponSo == null) return;

        _currentWeapon.CurrentAmmo += amount;

        if (_currentWeapon.CurrentAmmo > _currentWeaponSo.MagazineSize)
            _currentWeapon.CurrentAmmo = _currentWeaponSo.MagazineSize;

        UpdateAmmoText(_currentWeapon.CurrentAmmo, _currentWeaponSo.MagazineSize);
    }

    // ===================== CAMBIO DE ARMA =======================

    void HandleWeaponSwitch()
    {
        if (_weapons.Count == 0) return;
        if (Mouse.current == null && Keyboard.current == null) return;

        if (Time.time - _lastSwitchTime < switchWeaponCooldown)
            return;

        bool switched = false;

        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (scroll > 0f)
            {
                _currentWeaponIndex++;
                if (_currentWeaponIndex >= _weapons.Count) _currentWeaponIndex = 0;
                EquipWeapon(_currentWeaponIndex);
                switched = true;
            }
            else if (scroll < 0f)
            {
                _currentWeaponIndex--;
                if (_currentWeaponIndex < 0) _currentWeaponIndex = _weapons.Count - 1;
                EquipWeapon(_currentWeaponIndex);
                switched = true;
            }
        }

        if (switched)
            _lastSwitchTime = Time.time;
    }

    // ===================== DISPARO =======================

    void HandleShoot()
    {
        _timeSinceLastShot += Time.deltaTime;

        if (!_starterAssetsInputs.shoot) return;
        if (_currentWeaponSo == null || _currentWeapon == null) return;

        if (_timeSinceLastShot >= _currentWeaponSo.FireRate && _currentWeapon.CurrentAmmo > 0)
        {
            _currentWeapon.Shoot();
            BattlePressureManager.Instance?.ModifyPressure(1.5f);
            _animator.Play(SHOOT_STRING, 0, 0f);
            _audioSource.PlayOneShot(_currentWeaponSo.ShootSound);
            _timeSinceLastShot = 0f;

            UpdateAmmoText(_currentWeapon.CurrentAmmo, _currentWeaponSo.MagazineSize);
        }

        if (!_currentWeaponSo.IsAutomatic)
        {
            _starterAssetsInputs.ShootInput(false);
        }
    }

    // ===================== ZOOM =======================

    void HandleZoom()
    {
        if (!_currentWeaponSo) return;
        if (!_currentWeaponSo.CanZoom) return;

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

    // ===================== UTIL =======================

    void UpdateAmmoText(int current, int max)
    {
        ammoText.text = $"{current:D2}";
    }
}
