using System;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public enum AnimationType 
{
    idle = 0,
    run = 1,
    died = 2,
}

public class PlayerAnimController : NetworkBehaviour
{
    [SerializeField]private NetworkMecanimAnimator _mecanimAnimator;
    [Networked, OnChangedRender(nameof(SyncAnimation))] public AnimationType CurrentAnimation { get; private set; }

    private void SyncAnimation()
    {
        _mecanimAnimator.Animator.CrossFade(CurrentAnimation.ToString(), 0f);
    }

    public void SetAnimation(AnimationType type)
    {
        if(!HasInputAuthority) return;
        if (CurrentAnimation != type)
        {
            RPC_ChangeAnimationType(type);
        }
    }   

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_ChangeAnimationType(AnimationType type)
    {
        CurrentAnimation = type;
        _mecanimAnimator.Animator.CrossFade(CurrentAnimation.ToString(), 0f);
    }
}