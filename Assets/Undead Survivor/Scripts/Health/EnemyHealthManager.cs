
using Fusion;
using UnityEngine;

public class EnemyHealthManager : HealthManager
{
    public override void SubtractHP(int damage, PlayerRef player)
    {
        if (!_deathManager.IsDead)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            DamageSystem.Singleton.AddDamage(player, damage);
            if (CurrentHealth <= 0)
            {
                DamageSystem.Singleton.AddKill(player);
                _deathManager.Die();
            }
        }
    }
}
