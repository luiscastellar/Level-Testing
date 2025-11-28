using System;
using System.Collections;
using UnityEngine;

public class SpawnGate : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] Transform spawnLocation;

    PlayerHealth _player;
    
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
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
