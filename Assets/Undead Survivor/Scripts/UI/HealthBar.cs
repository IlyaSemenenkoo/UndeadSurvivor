using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : NetworkBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private GameLogic _gameLogic;
    private PlayerHealthManager _playerHealthManager;

    private void Awake()
    {
        if (_gameLogic.TryGetPlayer(Runner.LocalPlayer, out var playerObject))
        {
            _playerHealthManager = playerObject.GetBehaviour<PlayerHealthManager>();
            _slider.maxValue = _playerHealthManager.ReturnMaxHealth();
            _playerHealthManager.OnHpChangeEvent += SetHealth;
            
        }
    }

    private void SetHealth(PlayerRef playerRef, int health)
    {
        if (playerRef == Runner.LocalPlayer)
        {
            _slider.value = health;
        }
    }

    private void OnDestroy()
    {
        _playerHealthManager.OnHpChangeEvent -= SetHealth;
    }
}
