using Fusion;
using UnityEngine;


public abstract class HealthManager : NetworkBehaviour, IHealth
{
    [SerializeField] protected int _maxHealth = 100;
    [Networked] public int CurrentHealth { get; protected set; } = 20;

    
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            CurrentHealth = _maxHealth;
        }
        
    }

    public abstract void SubtractHP(int damage, PlayerRef player);


    public abstract void AddHP(int heal, PlayerRef player);
}
