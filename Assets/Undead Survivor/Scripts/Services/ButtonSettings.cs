using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSettings : NetworkBehaviour
{
    [SerializeField] private Button _button;
    
    private void OnEnable()
    {
        if (Runner.IsServer)
        {
            
        }
    }
}
