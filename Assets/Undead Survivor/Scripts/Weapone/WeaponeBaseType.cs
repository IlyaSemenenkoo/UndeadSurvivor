using UnityEngine;
using Fusion;

public abstract class WeaponeBaseType : NetworkBehaviour
{
    [SerializeField] protected AmmoHandler _ammoHandler;
    [SerializeField] protected Projectile _projectilePrefab;
    [SerializeField] protected Transform _shootPoint;
    [SerializeField] protected WeaponeSettings _settings;
    //[SerializeField] protected 
    protected float _bulletRotation;
    
    private float _currentCulldown = 0;

    public override void FixedUpdateNetwork()
    {
        if (_currentCulldown >= _settings.AttackCulldown)
        {
            if (GetInput(out NetworkInputData data))
            {
                if (data.BulletDirection != Vector2.zero)
                {
                    Debug.Log(data.BulletDirection.x + "," + data.BulletDirection.y);
                    if (!_ammoHandler.MagazineIsEmpty())
                    {
                        _currentCulldown = 0;
                        Shoot(data.BulletDirection);
                        _ammoHandler.Shoot();
                    }
                    else
                    {
                        _ammoHandler.ChangeMagazine();
                    }
                }
            }
        }
        else
        {
            _currentCulldown += Time.fixedDeltaTime;
        }
    }

    protected abstract void Shoot(Vector2 direction);


    protected void SpawnBullet(Vector2 direction)
    {
        _bulletRotation = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
        Runner.Spawn(_projectilePrefab, _shootPoint.position, Quaternion.Euler(0, 0, _bulletRotation), Object.InputAuthority,
            (runner, spawnedObject) =>
            {
                var bullet = spawnedObject.GetComponent<Projectile>();
                bullet.Initialize(direction, _settings.LifeTime, _settings.Damage, _settings.Speed);
            });
    }
    
}
