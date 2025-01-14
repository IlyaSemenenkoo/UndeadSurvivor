using Fusion;
using UnityEngine;

public class BombCollectible : BaseCollectibleType
{
    [SerializeField] private int _damage;
    [SerializeField] private int _damageRadius;
    [SerializeField] private LayerMask _layer;
    public override void MakeImpact(NetworkObject impactedObject)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, _damageRadius, _layer);
        foreach (var hit in hits)
        {
            hit.GetComponent<EnemyHealthManager>().SubtractHP(_damage, PlayerRef.None);
        }
        Destroy(gameObject);
    }
}
