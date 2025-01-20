using Fusion;
using UnityEngine;

public class SkinChangedManager : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(SyncAnimation))] private int SkinID { get; set; }
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimatorOverrideController[] _controllers;
    private string _skin = "Skin";


    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            var skinId = PlayerPrefs.GetInt(_skin);
            
            RPC_ChangedSkin(skinId);
        }
        SyncAnimation();
    }
    private void SyncAnimation()
    {
        _animator.runtimeAnimatorController = _controllers[SkinID];
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_ChangedSkin(int skinID)
    {
        SkinID = skinID;
    }
}
