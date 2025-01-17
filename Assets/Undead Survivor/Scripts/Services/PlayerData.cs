using Fusion;
using UnityEngine;

public struct PlayerData : INetworkStruct
{
    private  float _damage;
    private int _kill;
    private NetworkString<_64> _pLayerName;

    public void SetName(string name)
    {
        _pLayerName = name;
    }
    
    public void SetDamage(float damage)
    {
        _damage += damage;
    }

    public void SetKill()   
    {
        _kill++;
    }
    
    public string GetName()
    {
        return _pLayerName.ToString();
    }

    public float GetDamage()
    {
        return _damage;
    }

    public int GetKill()
    {
        return _kill;
    }
}
    

