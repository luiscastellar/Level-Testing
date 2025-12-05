using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] Transform turretHead;
    [SerializeField] Transform playerTargetPoint;
    [SerializeField] Transform turretSpawnPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate = 2f;
    [SerializeField] int damage = 2;
    
    PlayerHealth _player;

    void Start()
    {
        _player = FindFirstObjectByType<PlayerHealth>();
        StartCoroutine(Fire());
    }

    void Update()
    {
        turretHead.LookAt(playerTargetPoint);
    }

    IEnumerator Fire()
    {
        while (_player)
        {
            yield return new WaitForSeconds(fireRate);
            Projectile newProjectile = Instantiate(bulletPrefab, turretSpawnPoint.position, quaternion.identity).GetComponent<Projectile>(); 
            newProjectile.transform.LookAt(playerTargetPoint);
            newProjectile.Init(damage);
        }
    }
    
    
}
