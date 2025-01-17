using UnityEngine;

public class EnemyDeathManager : MonoBehaviour
{
    [SerializeField] private BaseAnimController _animController;
    
    public bool IsDead { get; private set; }

    private float _deathTime = 2f;
    public void Die()
    {
        if (!IsDead)
        {
            IsDead = true;
            if (_animController != null)
            {
                _animController.SetAnimation(AnimationType.died);
                gameObject.GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject, 2f);
            }
        }
    }
}
