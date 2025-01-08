using System.ComponentModel.Design;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine; 

public class EnemyMovement : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _enemyTransform;
    private Transform _target;
    [SerializeField] private EnemySettings _settings;

    private float lastAttackTime;
    
    [Networked, OnChangedRender(nameof(SyncRotation))] public Vector3 Rotation { get; private set;}
    
    private void SyncRotation()
    {
        _enemyTransform.transform.localScale = Rotation;
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.IsServer)
        {
            FindClosestPlayer();
        }

        if (_target != null)
        {
            if (Vector2.Distance(transform.position, _target.position) <= _settings.AttackRadius)
            {
                if (Time.time - lastAttackTime > _settings.AttackCulldown)
                {
                    AttackPlayer();
                }
            }
            MoveTowardsTarget();
            ChangeRotation();
        }
    }

    private void FindClosestPlayer()
    {
        Collider2D hit = Runner.GetPhysicsScene2D().OverlapCircle(this.transform.position, _settings.DetectionRadius, _settings.PlayerLayer);

        if (hit != null)
        {
            _target = hit.transform;
        }
        else
        {
            _target = null;
        }
        
        
    }

    private void MoveTowardsTarget()
    {
        Vector2 direction = (_target.position - this.transform.position).normalized;
        _rigidbody.velocity = direction * _settings.Speed;
    }
    
    private void ChangeRotation()
    {
        if(!Runner.IsServer) return;
        Vector3 TempRotation = Vector3.zero;
        if (_target.position.x < transform.position.x)
        {
            TempRotation = new Vector3(1, 1, 1);
        }
        else if (_target.position.x > transform.position.x)
        {
            TempRotation = new Vector3(-1, 1, 1);
        }

        if (TempRotation != Vector3.zero && TempRotation != Rotation)
        {
            Rotation = TempRotation;
            _enemyTransform.transform.localScale = TempRotation;
        }
    }

    private void AttackPlayer()
    {
        if(!Runner.IsServer) return;
         lastAttackTime = Time.time;

         if (_target.TryGetComponent(out HealthManager HealthManager))
         {
             HealthManager.SubtractHP(_settings.AttackDamage);
             Debug.Log($"Враг атакует игрока {_target.name}, наносит {_settings.AttackDamage} урона");
         }
    }
}
