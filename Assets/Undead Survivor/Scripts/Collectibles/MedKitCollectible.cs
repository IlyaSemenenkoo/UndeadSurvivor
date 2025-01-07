using System;
using UnityEngine;

public class MedKitCollectible : BaseCollectibleType
{
    [SerializeField] private int _health;
    
    public override void OnCollisionEnter(Collision other)
    {
        if(!Runner.IsServer) return;

        if (other.gameObject.TryGetComponent<HealthManager>(out HealthManager healthManager))
        {
            healthManager.AddHP(_health);
            Destroy(other.gameObject);
        }
    }
}
