using System.ComponentModel.Design;
using System.Linq;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine; 

public class EnemyMovement : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _enemyTransform;
    private Transform _target;
    [SerializeField] private EnemySettings _settings;
    [SerializeField] private EnemyDeathManager _deathManager;

    private float lastAttackTime;

    [Networked, OnChangedRender(nameof(SyncRotation))] public Vector3 Rotation { get; private set;}
    
    private void SyncRotation()
    {
        _enemyTransform.transform.localScale = Rotation;
    }

    public override void FixedUpdateNetwork()
    {
        if (_deathManager.IsDead)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }
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
        var results = new Collider2D[100];
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, _settings.DetectionRadius, results);

        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < size; i++)
        {
            var hit = results[i];
            if (hit != null && hit.TryGetComponent(out DeathManager deathManager) && !deathManager.IsDead)
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = hit.transform;
                }
            }
        }

        if (closestTarget != null)
        {
            _target = closestTarget;
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
            TempRotation = new Vector3(-1, 1, 1);
        }
        else if (_target.position.x > transform.position.x)
        {
            TempRotation = new Vector3(1, 1, 1);
        }

        if (TempRotation != Vector3.zero && TempRotation != Rotation)
        {
            Rotation = TempRotation;
            _enemyTransform.transform.localScale = TempRotation;
        }
    }

    private void AttackPlayer()
    {
        if(!HasStateAuthority) return;
         lastAttackTime = Time.time;

         if (_target.TryGetComponent(out HealthManager healthManager))
         {
             healthManager.SubtractHP(_settings.AttackDamage, _target.GetComponent<NetworkObject>().InputAuthority);
         }
    }
}
