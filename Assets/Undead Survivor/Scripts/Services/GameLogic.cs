using Fusion;
using UnityEngine;

public class GameLogic : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [Networked, Capacity(2)] private NetworkDictionary<PlayerRef, NetworkObject> _spawnedCharacters => default;

    public void PlayerJoined(PlayerRef player)
    {
        NetworkObject networkPlayerObject = null;
        if (HasStateAuthority)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % Runner.Config.Simulation.PlayerCount) * 3, 1, 0); 
            networkPlayerObject = Runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
        Debug.Log("Player joined");
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
