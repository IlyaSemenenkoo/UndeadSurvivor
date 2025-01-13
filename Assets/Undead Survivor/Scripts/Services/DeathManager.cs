using System;
using UnityEngine;

public class DeathManager : MonoBehaviour
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
            }
            Debug.Log($"{gameObject.name} is dead ");
        }
    }
}
