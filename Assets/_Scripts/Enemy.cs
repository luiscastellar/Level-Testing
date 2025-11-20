using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private FirstPersonController _player;
    NavMeshAgent _agent;

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
        _agent.SetDestination(_player.transform.position);
    }
}
