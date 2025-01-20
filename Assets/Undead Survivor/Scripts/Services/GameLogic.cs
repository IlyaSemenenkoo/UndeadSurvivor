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
                    if (_allReadySpawnedWeapone.Count > 0)
                    {
                        int randomIndex = Random.Range(0, _allReadySpawnedWeapone.Count);
                        int selectedWeaponIndex = _allReadySpawnedWeapone[randomIndex];
                        Debug.Log("Weapon spawned");
                        player.GetComponent<WeaponeService>().SettingsSetup(selectedWeaponIndex);
                        
                        _allReadySpawnedWeapone.RemoveAt(randomIndex);
                    }
                }
                );
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

    public GameObject GetSecondPlayer(PlayerRef playerRef)
    {
        GameObject result = null;
        foreach (var player in _spawnedCharacters)
        {
            if (!Equals(player.Key, playerRef))
            {
                result = player.Value.gameObject;
            }
        }
        return result;
    }
}
