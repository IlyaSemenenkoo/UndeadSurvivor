using Fusion;
using UnityEngine;

public class CollectibleServices : NetworkBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!HasInputAuthority) return;
        if (other.gameObject.TryGetComponent(out BaseCollectibleType baseCollectibleType))
        {
            RPC_SyncCollectible(baseCollectibleType, gameObject.GetComponent<NetworkObject>());
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SyncCollectible(BaseCollectibleType baseCollectibleType, NetworkObject player)
    {
        baseCollectibleType.MakeImpact(player);
    }
}
