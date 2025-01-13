using System;
using Fusion;
using UnityEngine;

public class PlayerHealthManager : HealthManager
{
    public event Action<int> OnHpChangeEvent;
    

    public override void SubtractHP(int damage, PlayerRef player)
    {
        if (!_deathManager.IsDead)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            OnHpChangeEvent?.Invoke(CurrentHealth);
            if (CurrentHealth <= 0)
            {
                _deathManager.Die();
            }
        }
    }
}
