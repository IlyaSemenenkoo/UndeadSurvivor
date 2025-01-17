using System;
using Fusion;
using UnityEngine;

public class PlayerHealthManager : HealthManager
{
    public event Action<PlayerRef, int> OnHpChangeEvent;
    [SerializeField] protected DeathManager _deathManager;

    public override void SubtractHP(int damage, PlayerRef player)
    {
        if (!_deathManager.IsDead)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            RPC_HpSync(player, CurrentHealth);
            if (CurrentHealth <= 0)
            {
                RPC_PlayerDeath(player);
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_PlayerDeath(PlayerRef player)
    {
        _deathManager.PlayDeath(player);
    }

    public override void AddHP(int heal, PlayerRef player)
    {
        if (!_deathManager.IsDead)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + heal, _maxHealth);
            RPC_HpSync(player, CurrentHealth);
        }
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_HpSync(PlayerRef player, int currentHealth)
    {
        OnHpChangeEvent?.Invoke(player, CurrentHealth);
    }  

    public int ReturnMaxHealth()
    {
        return _maxHealth;
    }
}
