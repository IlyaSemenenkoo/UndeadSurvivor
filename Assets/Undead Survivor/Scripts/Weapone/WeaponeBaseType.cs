using UnityEngine;
using Fusion;

public abstract class WeaponeBaseType : NetworkBehaviour
{
    [SerializeField] protected AmmoHandler _ammoHandler;
    [SerializeField] protected Projectile _projectilePrefab;
    [SerializeField] protected Transform _shootPoint;
    [SerializeField] protected int _damage;
    [SerializeField] protected int _bulletByShoot;
    [SerializeField] protected float _lifeTime;
    [SerializeField] protected float _attackCulldown;
    
    private float _currentCulldown;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            if (data.BulletDirection != Vector2.zero)
            {
                Shoot(data.BulletDirection);
            }
        } 
    }

    protected void Shoot(Vector2 direction)
    {
        if(!HasInputAuthority) return;

        if (!_ammoHandler.MagazineIsEmpty())
        {
            _ammoHandler.Shoot();
            SpawnBullet(direction);
        }
        else
        {
            _ammoHandler.ChangeMagazine();
        }
    }

    protected void SpawnBullet(Vector2 direction)
    {
        Runner.Spawn(_projectilePrefab, _shootPoint.position, Quaternion.identity, Object.InputAuthority,
            (runner, spawnedObject) =>
            {
                var bullet = spawnedObject.GetComponent<Projectile>();
                bullet.Initialize(direction, _lifeTime, _damage);
            });
    }
    
}
