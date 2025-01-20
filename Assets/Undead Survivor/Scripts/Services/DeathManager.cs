using Fusion;
using UnityEngine;

public class DeathManager : NetworkBehaviour
{
    [SerializeField] private BaseAnimController _animController;
    
    [Networked, OnChangedRender(nameof(DeadSync))]public bool IsDead { get; set; }

    private void DeadSync()
    {
        if (!HasInputAuthority) return;
        if (_animController != null)
        {
            RPC_SyncDeath();
            _animController.SetAnimation(AnimationType.Died);
            gameObject.GetComponent<Collider2D>().enabled = false;
            if (gameObject.GetComponent<PlayerMovement>() != null)
            {
                GetComponent<WeaponeService>().PlayerDead();
            }
        }
    }

    public void PlayDeath(PlayerRef playerRef)
    {
        if (Runner.LocalPlayer == playerRef && !IsDead)
        {
            VirtualCameraManager.Singleton.PlayerDead(Runner.LocalPlayer);
            IsDead = true;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SyncDeath()
    {
        IsDead = true;
    }
}
