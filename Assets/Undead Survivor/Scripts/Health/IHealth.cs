using System;

public interface IHealth
{
    public int CurrentHealth { get; }
    
    public void SubtractHP(int damage);
    public void AddHP(int healAmount);
}