using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class DamageSystem : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [Networked, Capacity(2)] private NetworkDictionary<PlayerRef, DamageData> _damagePlayerScore => default;
    
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
                Debug.LogError($"There should only ever be one instance of {nameof(VirtualCameraManager)}!");
            }
        }
    }
    public static DamageSystem _singleton;


    public void PlayerJoined(PlayerRef player)
    {
        if (HasStateAuthority)
        {
            _damagePlayerScore.Add(player, new DamageData());
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
        if (_damagePlayerScore.TryGet(player,out DamageData damageData))
        {
            damageData.Damage += damage;
        }
        else
        {
            _damagePlayerScore.Add(player, new DamageData(){Damage = damage});
        }
    }
    
    public void AddKill(PlayerRef player)
    {
        if (_damagePlayerScore.TryGet(player, out DamageData damageData))
        {
            damageData.Kill += 1;
        }
        else
        {
            _damagePlayerScore.Add(player, new DamageData(){Kill = 1});
        }
    }
}
