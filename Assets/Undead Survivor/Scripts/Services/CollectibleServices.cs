using Fusion;
using UnityEngine;

public class CollectibleServices : NetworkBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!HasStateAuthority)
        {
            return;
        }
        if (other.gameObject.TryGetComponent(out BaseCollectibleType baseCollectibleType))
        {
            baseCollectibleType.MakeImpact(GetComponent<NetworkObject>());
        }
    }
}
