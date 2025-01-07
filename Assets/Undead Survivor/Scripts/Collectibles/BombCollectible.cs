using System.Collections;
using System.Collections.Generic;
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
            
            Destroy(other.gameObject);
        }
    }

    private void Detonate()
    {
        Collider2D hits = Runner.GetPhysicsScene2D().OverlapCircle(this.transform.position, _damageRadius, _layer);
    }
}
