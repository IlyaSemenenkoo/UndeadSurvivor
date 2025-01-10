using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class BombCollectible : BaseCollectibleType
{
    [SerializeField] private int _damage;
    [SerializeField] private int _damageRadius;
    [SerializeField] private LayerMask _layer;
    public override void OnCollisionEnter(Collision other)
    {
        if(!Runner.IsServer) return;

        if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            Detonate();
            Destroy(other.gameObject);
        }
    }

    private void Detonate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, _damageRadius, _layer);
        foreach (var hit in hits)
        {
            hit.GetComponent<HealthManager>().SubtractHP(_damage, PlayerRef.None);
        }
    }
}
