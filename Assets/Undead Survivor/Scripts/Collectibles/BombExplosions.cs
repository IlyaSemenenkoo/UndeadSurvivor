using System;
using Fusion;
using UnityEngine;

public class BombExplosions : NetworkBehaviour
{
    [SerializeField] private int _damage;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!Runner.IsServer)
        {
            return;
        }
        if (collider.gameObject.TryGetComponent(out EnemyHealthManager _enemyHealthManager))
        {
            Debug.Log("Trigger");
            _enemyHealthManager.SubtractHP(_damage, PlayerRef.None);
        }
    }
}
