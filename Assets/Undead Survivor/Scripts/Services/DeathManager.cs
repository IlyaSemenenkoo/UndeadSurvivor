using System;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    private PlayerAnimController _playerAnimController;
    
    public bool IsDead { get; private set; }

    private void Awake()
    {
        _playerAnimController = GetComponent<PlayerAnimController>();
    }

    public void Die()
    {
        if (!IsDead)
        {
            IsDead = true;
            if (_playerAnimController != null)
            {
                _playerAnimController.SetAnimation(AnimationType.died);
            }
            Debug.Log($"{gameObject.name} is dead ");
        }
    }
}
