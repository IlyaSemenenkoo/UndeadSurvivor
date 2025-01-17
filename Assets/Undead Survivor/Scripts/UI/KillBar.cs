using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class KillBar : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _killText;
    [FormerlySerializedAs("_damageSystem")] [SerializeField] private PLayerDataSystem _pLayerDataSystem;

    private int _kills;

    private void OnEnable()
    {
        _pLayerDataSystem.OnKill += SetKill;
    }

    private void SetKill(PlayerRef playerRef)
    {
        if(playerRef == Runner.LocalPlayer)
        {
            _kills = _kills + 1;
            _killText.text = _kills.ToString();
        }
    }

    private void OnDisable()
    {
        _pLayerDataSystem.OnKill -= SetKill;
    }
}
