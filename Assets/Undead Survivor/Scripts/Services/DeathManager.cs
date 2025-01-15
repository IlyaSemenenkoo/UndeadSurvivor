using System;
using Fusion;
using UnityEngine;

public class DeathManager : NetworkBehaviour
{
    [SerializeField] private BaseAnimController _animController;
    
    public bool IsDead { get; private set; }

    public void Die()
    {
        if (!IsDead)
        {
            IsDead = true;
            if (_animController != null)
            {
                _animController.SetAnimation(AnimationType.died);
                gameObject.GetComponent<Collider2D>().enabled = false;
                if (gameObject.GetComponent<PlayerMovement>() != null)
                {
                    GetComponent<WeaponeService>().PlayerDead();
                    VirtualCameraManager.Singleton.PlayerDead(Runner.LocalPlayer);
                }
            }
        }
    }
}
