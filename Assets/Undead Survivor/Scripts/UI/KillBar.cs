using TMPro;
using UnityEngine;

public class KillBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _killText;
    
    private int _kills;

    private void OnEnable()
    {
        
    }

    private void SetKill()
    {
        _kills++;
        _killText.text = _kills.ToString();
    }

    private void OnDisable()
    {

    }
}
