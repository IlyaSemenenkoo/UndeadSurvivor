using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using Fusion.Addons.Physics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private JoystickForMovement _joystickForMovement;
    [SerializeField] private JoystickForMovement _joystickForShoot;
    [SerializeField] private Button _button;
    private string _gameMode = "GameMode";
    private string _lobbyName = "LobbyName";
    private string _menuScene = "MenuScene";

    private void Awake()
    {
        GameMode mode = Enum.Parse<GameMode>(PlayerPrefs.GetString(_gameMode));
        string lobbyName = PlayerPrefs.GetString(_lobbyName);
        StartGame(mode, lobbyName);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    { }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();
        
        data.MoveDirection = _joystickForMovement.ReturnVectorDirection().normalized;
        data.BulletDirection = _joystickForShoot.ReturnVectorDirection().normalized;
        
        input.Set(data);
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        SceneManager.LoadScene(_menuScene);
    }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){ }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){ }
    
    private NetworkRunner _runner;

    async void StartGame(GameMode mode, string lobbyName)
    {
        _runner = new GameObject("NetworkRunner").AddComponent<NetworkRunner>();
        //_runner.gameObject.AddComponent<RunnerSimulatePhysics2D>().ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateAlways;
        _runner.ProvideInput = true;
        _runner.AddCallbacks(this);
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid) {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = lobbyName,
            Scene = scene,
            SceneManager = _runner.gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
        if (mode == GameMode.Host)
        {
            _button.gameObject.SetActive(true);
        }
    }
}