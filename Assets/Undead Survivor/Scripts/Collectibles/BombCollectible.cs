using System.Linq;
using Fusion;
using UnityEngine;

public class BombCollectible : BaseCollectibleType
{
    [SerializeField] private float _damageRadius;
    [SerializeField] private int _damage;
    [SerializeField] private LayerMask _layer;
    public override void MakeImpact(NetworkObject impactedObject)
    {
        var results = new Collider2D[100];
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, _damageRadius, results);

        Debug.Log(size);
        if (size < 0)
        {
            Debug.Log("Nothing");
        }
        else
        {
            var list = results.Where(result => result != null).ToList();
            foreach (Collider2D col in list)
            {
                Debug.Log("Collided with " + col.gameObject.name);
        
                if (col.TryGetComponent(out EnemyHealthManager _enemyHealthManager))
                {
                    _enemyHealthManager.SubtractHP(_damage, impactedObject.InputAuthority);
                    Debug.Log("Zombie hit");
                }
            }
        }
        
        Destroy(gameObject, 0.3f);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}
