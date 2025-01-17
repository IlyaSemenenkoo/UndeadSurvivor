using System;
using Fusion;
using UnityEngine;

public class PLayerDataSystem : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private EndGameUI _endGameUI;
    [Networked, Capacity(2)] private NetworkDictionary<PlayerRef, PlayerData> _playerData => default;
    private int _deadPLayer;
    private string _playerDeadResult = "Dead";
    private string _playerWinResult = "Win";
    
    public event Action<PlayerRef> OnKill;
    
    public static PLayerDataSystem Singleton
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
    public static PLayerDataSystem _singleton;

    private void Awake()
    {
        Singleton = this;
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (HasStateAuthority)
        {
            _playerData.Add(player, new PlayerData( ));
        }

        if (_playerData[player].GetName() != "")
        {
            SetName(Runner.LocalPlayer, PlayerPrefs.GetString("Name"));
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (HasStateAuthority)
        {
            if (_playerData.ContainsKey(player))
            {
                _playerData.Remove(player);
            }
        }
    }

    public void SetName(PlayerRef player, string playerName)
    {
        var playerData = _playerData.Get(player);
        playerData.SetName(playerName);
        _playerData.Remove(player);
        _playerData.Add(player, playerData);
    }

    public void AddDamage(PlayerRef player, float damage)
    {
        var playerData = _playerData.Get(player);
        playerData.SetDamage(damage);
        _playerData.Remove(player);
        _playerData.Add(player, playerData);
    }
    
    public void AddKill(PlayerRef player)
    {
        var playerData = _playerData.Get(player);
        playerData.SetKill();
        _playerData.Remove(player);
        _playerData.Add(player, playerData);
        RPC_KillSync(player);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_KillSync(PlayerRef player)
    {
        OnKill?.Invoke(player);
    }

    

    public void PlayerDead(PlayerRef player)
    {
        if(!HasStateAuthority) return;
        _deadPLayer++;
        if (_deadPLayer == _playerData.Count)
        {
            RPC_EndGame(_playerDeadResult);
        }
    }

    public void TimeEnded()
    {
        if(!HasStateAuthority) return;
        RPC_EndGame(_playerWinResult);
    }

    
    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    private void RPC_EndGame(string result)
    {
        EndGame(result);
    }

    private void EndGame(string result)
    {
        _endGameUI.gameObject.SetActive(true);
        _endGameUI.Initialize(result, _playerData);
    }
}
