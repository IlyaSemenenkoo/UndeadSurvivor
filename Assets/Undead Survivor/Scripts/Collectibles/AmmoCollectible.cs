using Fusion;
using UnityEngine;

public class AmmoCollectible : BaseCollectibleType
{
    [SerializeField] private int _ammo;
    public override void MakeImpact(NetworkObject impactedObject)
    {
        Debug.Log(_ammo);
        impactedObject.GetComponent<AmmoHandler>().AddAmmo(_ammo);
        Destroy(gameObject);
    }
}
