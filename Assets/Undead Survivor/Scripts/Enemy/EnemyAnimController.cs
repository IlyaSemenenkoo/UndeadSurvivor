using System;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimController : BaseAnimController
{
    [SerializeField] protected NetworkMecanimAnimator _mecanimAnimator;
    [Networked, OnChangedRender(nameof(SyncAnimation))] public AnimationType CurrentAnimation { get; private set; }

    protected override void SyncAnimation()
    {
        _mecanimAnimator.Animator.CrossFade(CurrentAnimation.ToString(), 0f);
    }

    public override void SetAnimation(AnimationType type)
    {
        if(!Runner.IsServer) return;
        if (CurrentAnimation != type)
        {
            CurrentAnimation = type;
            _mecanimAnimator.Animator.CrossFade(CurrentAnimation.ToString(), 0f);
        }
    }   
}