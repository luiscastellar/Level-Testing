using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float radius = 1.7f;
    [SerializeField] int damage = 2;

    void Start()
    {
        Explode();
    }

    void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hitCollider in hitColliders)
        {
            PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();

            if (!playerHealth) continue;
            
            playerHealth.TakeDamage(damage);
            
            break;
        }
    }
}
