using System;

public interface IHealth
{
    public int CurrentHealth { get; }
    
    event Action<int, int> OnHealthChanged;
    
    public void SubtractHP(int damage);
    public void AddHP(int healAmount);
}