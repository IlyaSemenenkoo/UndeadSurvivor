using System;
using Fusion;

public interface IHealth
{
    public int CurrentHealth { get; }
    
    public void SubtractHP(int damage, PlayerRef player);
    public void AddHP(int healAmount, PlayerRef player);
}