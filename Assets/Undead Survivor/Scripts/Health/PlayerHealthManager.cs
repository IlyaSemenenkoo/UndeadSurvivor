using System;

public class PlayerHealthManager : HealthManager
{
    public event Action<int> OnHpChangeEvent;
    
    protected override void SyncHp()
    {
        if (HasInputAuthority)
        {
            OnHpChangeEvent?.Invoke(CurrentHealth);
        }
    }
}
