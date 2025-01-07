using System;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public enum AnimationTypeEnemy 
{
    run = 0,
    died = 1,
}

public class EnemyAnimController : NetworkBehaviour
{
    [SerializeField]private NetworkMecanimAnimator _mecanimAnimator;
    [Networked, OnChangedRender(nameof(SyncAnimation))] public AnimationTypeEnemy CurrentAnimation { get; private set; }

    private void SyncAnimation()
    {
        _mecanimAnimator.Animator.CrossFade(CurrentAnimation.ToString(), 0f);
    }

    public void SetAnimation(AnimationTypeEnemy type)
    {
        if(!HasInputAuthority) return;
        if (CurrentAnimation != type)
        {
            RPC_ChangeAnimationType(type);
        }
    }   

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_ChangeAnimationType(AnimationTypeEnemy type)
    {
        CurrentAnimation = type;
        _mecanimAnimator.Animator.CrossFade(CurrentAnimation.ToString(), 0f);
    }
}