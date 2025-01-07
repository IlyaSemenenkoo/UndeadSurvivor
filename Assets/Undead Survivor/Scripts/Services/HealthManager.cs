using System;
using Fusion;
using UnityEngine;


public class HealthManager : NetworkBehaviour, IHealth
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private DeathManager _deathManager;
    [Networked] public int CurrentHealth { get; private set; } = 20;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            CurrentHealth = _maxHealth;
        }
        
    }

    public void SubtractHP(int damage)
    {
        if (!_deathManager.IsDead)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            if (CurrentHealth <= 0)
            {
                _deathManager.Die();
            }
        }
    }

    public void AddHP(int heal)
    {
        if (!_deathManager.IsDead)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + heal, _maxHealth);
        }
    }
}
