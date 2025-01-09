using Unity.VisualScripting;
using UnityEngine;

public class AmmoCollectible : BaseCollectibleType
{
    [SerializeField] private int _ammo;
    public override void OnCollisionEnter(Collision other)
    {
        if(!Runner.IsServer) return;

        if (other.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.GetComponentInChildren<SpriteRenderer>().gameObject.GetComponentInChildren<AmmoHandler>().AddAmmo(_ammo);
            Destroy(other.gameObject);
        }
    }
}
