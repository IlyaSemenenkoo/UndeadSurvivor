using System;
using Fusion;
using TMPro;
using UnityEngine;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private GameLogic _gameLogic;
    private AmmoHandler _ammoHandler;

    private void OnEnable()
    {
        _gameLogic.OnPlayerJoined += PlayerJoined;
        // make a condition of null
        _ammoHandler.OnAmmoChangedEvent += SetAmmo;
    }

    private void SetAmmo(int ammo, int magazineAmmo)
    {
        
        _ammoText.text = $"{ammo} / {magazineAmmo}";
    }
    
    private void OnDestroy()
    {
        // make a condition of null
        _ammoHandler.OnAmmoChangedEvent -= SetAmmo;
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (_gameLogic.TryGetPlayer(player, out var playerObject))
        {
            _ammoHandler = playerObject.GetBehaviour<AmmoHandler>();
            _ammoHandler.OnAmmoChangedEvent += SetAmmo;
        }
    }
}

