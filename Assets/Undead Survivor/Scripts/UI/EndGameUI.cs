using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : NetworkBehaviour
{
    [SerializeField] private PlayerPanel[] _infoPanels;
    [SerializeField] private GameObject _overlay;
    [SerializeField] private GameObject _joystick;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private GameObject _endGameCanvas ;
    [SerializeField] private PlayerDataSystem _playerDataSystem;
    
    public void Initialize(string resultText, NetworkDictionary<PlayerRef, PlayerData> data)
    {
        _resultText.text = resultText;
        if (!HasStateAuthority) return;
        int i = 0;
        var playerKey = Runner.ActivePlayers;
        foreach (var player in playerKey)
        {
            var playerData = data.Get(player);
            RPC_SyncParent(playerData.GetName(), playerData.GetDamage(), playerData.GetKill(), i);
            i++;
        }
    }

    public void OnUI()
    {
        _endGameCanvas.SetActive(true);
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_SyncParent(string name, float damage, float kill, int index)
    {
        OnInfoPanels(true, index);
        _infoPanels[index].Initialize(name, damage, kill);
    }

    private void OnInfoPanels(bool state, int index)
    {
        _overlay.SetActive(false);
        _joystick.SetActive(false);
        _infoPanels[index].gameObject.SetActive(state);
    }
    
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
}
