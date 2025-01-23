using Fusion;

public abstract class BaseCollectibleType : NetworkBehaviour
{
    public abstract void MakeImpact(NetworkObject impactedObject);
}
