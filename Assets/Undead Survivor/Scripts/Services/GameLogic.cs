using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameLogic : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private GameObject _overlay;
    [Networked, Capacity(2)] private NetworkDictionary<PlayerRef, NetworkObject> _spawnedCharacters => default;
    List<int> _allReadySpawnedWeapone = new List<int>{0, 1, 2};
    public bool TryGetPlayer(PlayerRef playerRef, out NetworkObject networkObject)
    {
        return _spawnedCharacters.TryGet(playerRef, out networkObject);
    }
    public void PlayerJoined(PlayerRef player)
    {
        NetworkObject networkPlayerObject = null;
        if (HasStateAuthority)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % Runner.Config.Simulation.PlayerCount) * 3, 1, 0); 
            networkPlayerObject = Runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player,
                (runner, spawnedObject) =>
                {
                    var player = spawnedObject;
                    int num = Random.Range(0, _allReadySpawnedWeapone.Count-1);
                    player.GetComponent<WeaponeService>().SettingsSetup(_allReadySpawnedWeapone[num]);
                    if (_allReadySpawnedWeapone.Contains(num))
                    {
                        _allReadySpawnedWeapone.Remove(num);
                    }
                }
                );
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
        _overlay.SetActive(true);
    }

    public void PlayerLeft(PlayerRef player)
    {
        if(!HasStateAuthority)
            return;
        
        if (_spawnedCharacters.TryGet(player, out NetworkObject networkObject))
        {
            Runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
}
