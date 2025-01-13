
using Fusion;

public struct DamageData : INetworkStruct
{
    public float Damage {
        get
        {
            return Damage;
        }
        set
        {
            Damage = Damage + value;
        } 
    }
    public int Kill 
    {
        get
        {
            return Kill;
        }
        set
        {
            Kill = Kill + value;
        } 
    }
}
    

