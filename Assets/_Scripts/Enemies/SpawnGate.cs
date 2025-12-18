using System;
using System.Collections;
using UnityEngine;

public class SpawnGate : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform spawnLocation;
    
    [SerializeField] float minSpawnTime = 1.5f;
    [SerializeField] float maxSpawnTime = 5f;
    
    PlayerHealth _player;
    
    float GetSpawnTime()
    {
        float pressure01 = BattlePressureManager.Instance.currentPressure / 100f;
        return Mathf.Lerp(maxSpawnTime, minSpawnTime, pressure01);
    }
    
    void Start()
    {
        _player = FindFirstObjectByType<PlayerHealth>();
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (_player)
        {
            Instantiate(enemyPrefab, spawnLocation.position, spawnLocation.rotation);
            yield return new WaitForSeconds(GetSpawnTime());
        }
    }
}
