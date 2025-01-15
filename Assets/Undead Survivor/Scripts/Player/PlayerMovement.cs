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
            VirtualCameraManager.Singleton.FollowThis(this.gameObject);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data) && _playerAnimController.CurrentAnimation != AnimationType.died)
        {
            ChangeRotation(data.MoveDirection);
            _rigidbody.velocity = data.MoveDirection * _speed;
            if (data.MoveDirection != Vector2.zero)
            {
                _playerAnimController.SetAnimation(AnimationType.run);
            }
            else
            {
                _playerAnimController.SetAnimation(AnimationType.idle);
            }
        }
        else if (_playerAnimController.CurrentAnimation == AnimationType.died)
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }

    private void ChangeRotation(Vector2 direction)
    {
        Vector3 TempRotation = Vector3.zero;
        if(!HasInputAuthority) return;
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

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SyncRotation(Vector3 rotation)
    {
        Rotation = rotation;
        _transform.transform.localScale = rotation;
    }
}