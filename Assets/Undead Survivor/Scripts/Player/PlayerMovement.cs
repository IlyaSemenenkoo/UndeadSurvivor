using System;
using Fusion;
using UnityEngine;


public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private PlayerAnimController _playerAnimController;
    [SerializeField] private Transform _transform;
    
    [Networked, OnChangedRender(nameof(SyncRotation))] public Vector3 Rotation { get; private set;}
    
    private void SyncRotation()
    {
        _transform.transform.localScale = Rotation;
    }   
    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            VirtualCameraManager._singleton.FollowThis(this.gameObject);
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (_playerAnimController.CurrentAnimation != AnimationType.Died)
        {
            MovePlayer();
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }

    private void MovePlayer()
    {
        if (GetInput(out NetworkInputData data))
        {
            ChangeRotation(data.MoveDirection);
            _rigidbody.velocity = data.MoveDirection * _speed;
            if (data.MoveDirection != Vector2.zero)
            {
                _playerAnimController.SetAnimation(AnimationType.Run);
            }
            else
            {
                _playerAnimController.SetAnimation(AnimationType.Idle);
            }
        }
    }
    
    private void ChangeRotation(Vector2 direction)
    {
        Vector3 tempRotation = Vector3.zero;
        if (!HasInputAuthority) return;
        if (direction.x < 0)
        {
            tempRotation = new Vector3(-1, 1, 1);
        }
        else if (direction.x > 0)
        {
            tempRotation = new Vector3(1, 1, 1);
        }

        if (tempRotation != Vector3.zero && tempRotation != Rotation)
        {
            RPC_SyncRotation(tempRotation);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SyncRotation(Vector3 rotation)
    {
        Rotation = rotation;
        _transform.transform.localScale = rotation;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Collision enter");
        }
    }
}