
using Fusion;
using UnityEngine;

public class EnemyHealthManager : HealthManager
{
    [SerializeField] protected EnemyDeathManager _deathManager;
    public override void SubtractHP(int damage, PlayerRef player)
    {
        if (!_deathManager.IsDead)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            PLayerDataSystem.Singleton.AddDamage(player, damage);
            if (CurrentHealth <= 0)
            {
                PLayerDataSystem.Singleton.AddKill(player);
                _deathManager.Die();
            }
        }
    }

    public override void AddHP(int heal, PlayerRef player)
    { }
}
