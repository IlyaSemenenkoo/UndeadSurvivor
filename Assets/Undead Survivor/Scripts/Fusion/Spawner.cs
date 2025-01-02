using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using Fusion.Addons.Physics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private JoystickForMovement _joystickForMovement;
    [SerializeField] private JoystickForMovement _joystickForShoot;

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
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
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

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = new GameObject("NetworkRunner").AddComponent<NetworkRunner>();
        _runner.gameObject.AddComponent<RunnerSimulatePhysics2D>().ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateAlways;
        _runner.ProvideInput = true;
        _runner.AddCallbacks(this);
        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid) {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = _runner.gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    
    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0,0,200,40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0,40,200,40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }
}