using Fusion;
using UnityEngine;

public class MedKitCollectible : BaseCollectibleType
{
    [SerializeField] private int _health;
    
    public override void MakeImpact(NetworkObject impactedObject)
    {
        impactedObject.GetComponent<PlayerHealthManager>().AddHP(_health, impactedObject.InputAuthority);
        Destroy(gameObject);
    }
}
