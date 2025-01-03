using UnityEngine;
using Fusion;

public abstract class WeaponeBaseType : NetworkBehaviour
{
    [SerializeField] private AmmoHandler _ammoHandler;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private int _damage;
    [SerializeField] private int _bulletByShoot;
    [SerializeField] private int _shootDistance;
    [SerializeField] private float _attackCulldown;
    
    private float _currentCulldown;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            if (data.BulletDirection != Vector2.zero)
            {
                Shoot();
            }
        } 
    }

    private void Shoot()
    {
        if(!HasInputAuthority) return;

        if (!_ammoHandler.MagazineIsEmpty())
        {
            _ammoHandler.Shoot();
        }
        else
        {
            _ammoHandler.ChangeMagazine();
        }
    }

    
}
