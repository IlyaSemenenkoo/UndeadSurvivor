
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyHealthManager : HealthManager
{
    [SerializeField] protected EnemyDeathManager _enemyDeathManager;
    public override void SubtractHP(int damage, PlayerRef player)
    {
        if (!_enemyDeathManager.IsDead)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            PlayerDataSystem._singleton.AddDamage(player, damage);
            if (CurrentHealth <= 0)
            {
                PlayerDataSystem._singleton.AddKill(player);
                _enemyDeathManager.Die();
            }
        }
    }

    public override void AddHP(int heal, PlayerRef player)
    { }
}
