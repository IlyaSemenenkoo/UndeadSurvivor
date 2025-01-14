using Fusion;
using TMPro;
using UnityEngine;

public class AmmoBar : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private GameLogic _gameLogic;
    private AmmoHandler _ammoHandler;

    private void Awake()
    {
        if (_gameLogic.TryGetPlayer(Runner.LocalPlayer, out var playerObject))
        {
            Debug.Log("Player joined");
            _ammoHandler = playerObject.GetBehaviour<AmmoHandler>();
            _ammoHandler.OnAmmoChangedEvent += SetAmmo;
            _ammoHandler.StartSync();
        }
    }

    private void SetAmmo(int ammo, int magazineAmmo)
    {
        _ammoText.text = $"{ammo} / {magazineAmmo}";
    }
    
    private void OnDestroy()
    {
        _ammoHandler.OnAmmoChangedEvent -= SetAmmo;
    }
}

