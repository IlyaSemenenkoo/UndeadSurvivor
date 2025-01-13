using Fusion;
using UnityEngine;


public abstract class HealthManager : NetworkBehaviour, IHealth
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] protected DeathManager _deathManager;
    [Networked, OnChangedRender(nameof(SyncHp))] public int CurrentHealth { get; protected set; } = 20;
    protected abstract void SyncHp();

    
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
