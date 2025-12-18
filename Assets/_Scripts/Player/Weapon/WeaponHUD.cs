using System.Collections.Generic;
using UnityEngine;

public class WeaponHUD : MonoBehaviour
{
    [SerializeField] WeaponIconUI weaponIconPrefab;
    [SerializeField] Transform container;

    List<WeaponIconUI> _icons = new List<WeaponIconUI>();

    public void AddWeapon(WeaponSO weaponSo)
    {
        WeaponIconUI icon = Instantiate(weaponIconPrefab, container);
        icon.Setup(weaponSo.Icon);
        _icons.Add(icon);
        UpdateSelection(0);
    }

    public void UpdateSelection(int selectedIndex)
    {
        for (int i = 0; i < _icons.Count; i++)
        {
            _icons[i].SetSelected(i == selectedIndex);
        }
    }
}
