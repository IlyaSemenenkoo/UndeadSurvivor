using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class EnemyMovement : NetworkBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private NetworkRigidbody2D _networkRigidbody;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform _enemyTransform;
    [SerializeField] private float _detectionRadius;
    private Transform _target;
    
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
            MoveTowardsTarget();
        }
    }

    private void FindClosestPlayer()
    {
        Collider2D hit = Runner.GetPhysicsScene2D().OverlapCircle(this.transform.position, _detectionRadius, _playerLayer);

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
        _networkRigidbody.Teleport(_networkRigidbody.RBPosition + new Vector3(direction.x * _speed, direction.y * _speed, 0) * Time.fixedDeltaTime, null);
    }
    
    private void ChangeRotation(Vector2 direction)
    {
        if(!Runner.IsServer) return;
        Vector3 TempRotation = Vector3.zero;
        if (direction.x < 0)
        {
            TempRotation = new Vector3(-1, 1, 1);
        }
        else if (direction.x > 0)
        {
            TempRotation = new Vector3(1, 1, 1);
        }

        if (TempRotation != Vector3.zero && TempRotation != Rotation)
        {
            RPC_SyncRotation(TempRotation);
        }
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    private void RPC_SyncRotation(Vector3 rotation)
    {
        Rotation = rotation;
        _enemyTransform.transform.localScale = rotation;
    }
}
