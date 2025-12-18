using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private FirstPersonController _player;
    NavMeshAgent _agent;

    [SerializeField] float minSpeed = 2f;
    [SerializeField] float maxSpeed = 6f;

    private float _actualPressure;
    
    const string PLAYER_STRING = "Player";
    
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        _player = FindFirstObjectByType<FirstPersonController>();
    }

    void FixedUpdate()
    {
        if (!_player) return;
        
        _actualPressure = BattlePressureManager.Instance.currentPressure / 100f;
        _agent.speed = Mathf.Lerp(minSpeed, maxSpeed, _actualPressure);
        
        _agent.SetDestination(_player.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_STRING))
        {
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
            enemyHealth.SelfDestruct();
        }
    }
}
