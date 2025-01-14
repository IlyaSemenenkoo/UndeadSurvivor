using Fusion;
using UnityEngine;


public abstract class HealthManager : NetworkBehaviour, IHealth
{
    [SerializeField] protected int _maxHealth = 100;
    [SerializeField] protected DeathManager _deathManager;
    [Networked] public int CurrentHealth { get; protected set; } = 20;
    
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            CurrentHealth = _maxHealth;
        }
        
    }

    public abstract void SubtractHP(int damage, PlayerRef player);


    public void AddHP(int heal)
    {
        if (!_deathManager.IsDead)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + heal, _maxHealth);
        }
    }
}
