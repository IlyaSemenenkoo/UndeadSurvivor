using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class DamageSystem : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [Networked, Capacity(2)] private NetworkDictionary<PlayerRef, DamageData> _damagePlayerScore => default;
    
    public event Action<PlayerRef> OnKill;
    
    public static DamageSystem Singleton
    {
        get => _singleton;
        set
        {
            if (value == null)
                _singleton = null;
            else if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Destroy(value);
            }
        }
    }
    public static DamageSystem _singleton;

    private void Awake()
    {
        Singleton = this;
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (HasStateAuthority)
        {
            _damagePlayerScore.Add(player, new DamageData( ));
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (HasStateAuthority)
        {
            if (_damagePlayerScore.ContainsKey(player))
            {
                _damagePlayerScore.Remove(player);
            }
        }
    }

    public void AddDamage(PlayerRef player, float damage)
    {
        var damageData = _damagePlayerScore.Get(player);
        damageData.SetDamage(damage);
    }
    
    public void AddKill(PlayerRef player)
    {
        var damageData = _damagePlayerScore.Get(player);
        damageData.SetKill();
        RPC_KillSync(player);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_KillSync(PlayerRef player)
    {
        OnKill?.Invoke(player);
    }   

}
