
using Fusion;


public struct DamageData : INetworkStruct
{
    public float damage;
    public int kill;

    public void SetDamage(float damage)
    {
        this.damage += damage;
    }

    public void SetKill()
    {
        this.kill += 1;
    }

    public float GetDamage()
    {
        return damage;
    }

    public int GetKill()
    {
        return kill;
    }
}
    

