using System;
using Fusion;
using UnityEngine;

public class PlayerHealthManager : HealthManager
{
    [SerializeField] private DeathManager _deathManager;
    public event Action<PlayerRef, int> OnHpChangeEvent;
    

    public override void SubtractHP(int damage, PlayerRef player)
    {
        if (!_deathManager.IsDead)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            RPC_HpSync(player, CurrentHealth);
            if (CurrentHealth <= 0)
            {
                RPC_PlayerDeath(player);
                PlayerDataSystem._singleton.PlayerDead(player);
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
