using System;
using Fusion;
using UnityEngine;

public class PlayerHealthManager : HealthManager
{
    public event Action<PlayerRef, int> OnHpChangeEvent;
    

    public override void SubtractHP(int damage, PlayerRef player)
    {
        if (!_deathManager.IsDead)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            RPC_HpSync(player, CurrentHealth);
            if (CurrentHealth <= 0)
            {
                _deathManager.Die();
            }
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
