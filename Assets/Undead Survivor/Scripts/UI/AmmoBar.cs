using System;
using Fusion;
using TMPro;
using UnityEngine;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private GameLogic _gameLogic;
    private AmmoHandler _ammoHandler;

    private void Awake()
    {
        _gameLogic.OnPlayerJoined += PlayerJoined;
    }

    private void OnEnable()
    {
        // make a condition of null
        _ammoHandler.OnAmmoChangedEvent += SetAmmo;
    }

    private void SetAmmo(int ammo, int magazineAmmo)
    {
        
        _ammoText.text = $"{ammo} / {magazineAmmo}";
    }
    
    private void OnDisable()
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

            Debug.Log($"Player {playerObject.name} joined");
        }
        else
        {
            Debug.LogError("Player is not in game logic");
        }
    }
}

