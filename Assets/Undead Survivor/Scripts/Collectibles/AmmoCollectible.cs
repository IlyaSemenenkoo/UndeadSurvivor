using Fusion;
using UnityEngine;

public class AmmoCollectible : BaseCollectibleType
{
    [SerializeField] private int _ammo;
    public override void MakeImpact(NetworkObject impactedObject)
    {
        impactedObject.GetComponent<AmmoHandler>().AddAmmo(_ammo);
        Destroy(gameObject);
    }
}
