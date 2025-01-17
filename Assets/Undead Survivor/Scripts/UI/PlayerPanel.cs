using TMPro;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _playerDamage;
    [SerializeField] private TextMeshProUGUI _playerKills;
    
    public void Initialize(string playerName, float playerDamage, float playerKills)
    {
        _playerName.text = playerName;
        _playerDamage.text = playerDamage.ToString();
        _playerKills.text = playerKills.ToString();
    }
}
