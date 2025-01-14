using Fusion;
using TMPro;
using UnityEngine;

public class KillBar : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _killText;
    [SerializeField] private DamageSystem _damageSystem;

    private int _kills;

    private void OnEnable()
    {
        _damageSystem.OnKill += SetKill;
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
        _damageSystem.OnKill -= SetKill;
    }
}
