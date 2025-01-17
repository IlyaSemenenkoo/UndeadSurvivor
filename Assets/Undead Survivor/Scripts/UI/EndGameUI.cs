using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef _infoPanelPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _ovarlay;
    [SerializeField] private GameObject _joystick;
    [SerializeField] private TextMeshProUGUI _resultText;
    
    [SerializeField] private PLayerDataSystem _playerDataSystem;

    private float _shiftOnY = -257;

    private void OnEnable()
    {
        _ovarlay.SetActive(false);
        _joystick.SetActive(false);
    }
    
    public void Initialize(string resultText, NetworkDictionary<PlayerRef, PlayerData> data)
    {
        _resultText.text = resultText;
        var playerKey = Runner.ActivePlayers;
        foreach (var player in playerKey)
        {
            var playerData = data.Get(player);
            Runner.Spawn(_infoPanelPrefab, _spawnPoint.position, Quaternion.identity, Object.StateAuthority,
                (runner, spawnedObject) =>
                {
                    spawnedObject.transform.parent = transform;
                    spawnedObject.GetComponent<PlayerPanel>().Initialize(playerData.GetName(), playerData.GetDamage(), playerData.GetKill());
                });
            _spawnPoint.position += Vector3.up * _shiftOnY;
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
}
