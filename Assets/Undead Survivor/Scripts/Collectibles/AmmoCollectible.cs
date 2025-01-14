using System;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoCollectible : BaseCollectibleType
{
    [SerializeField] private int _ammo;
    public override void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collectible entered " + other.collider.gameObject.name);
        if(!Runner.IsServer) return;

        if (!other.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            return;
        }

        playerMovement.GetComponentInChildren<SpriteRenderer>().gameObject.GetComponentInChildren<AmmoHandler>().AddAmmo(_ammo);
        Destroy(other.gameObject);
    }
}
