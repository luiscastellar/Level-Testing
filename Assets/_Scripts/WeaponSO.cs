using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public GameObject WeaponPrefab;
    public GameObject HitVFXPrefab;
    
    public int Damage = 1;
    
    public float FireRate = .5f;
    public float ZoomAmount = 10f;
    public float ZoomRotationSpeed = .3f;
    
    public bool IsAutomatic = false;
    public bool CanZoom = false;
}
