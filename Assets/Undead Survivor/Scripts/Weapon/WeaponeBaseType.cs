using UnityEngine;
using Fusion;

public abstract class WeaponeBaseType : NetworkBehaviour
{
    [SerializeField] protected Transform _shootPoint;
    
    protected Projectile _projectilePrefab;
    protected float _bulletRotation;
    private float _currentCulldown = 0;
    private float _lifeTime;
    private int _damage;
    private float _speed;

    public void Initialize(Projectile ProjectilePrefab, float LifeTime, float Speed, int Damage)
    {
        _projectilePrefab = ProjectilePrefab;
        _lifeTime = LifeTime;
        _damage = Damage;
        _speed = Speed;
    }

    public abstract void Shoot(Vector2 direction);


    protected void SpawnBullet(Vector2 direction)
    {
        _bulletRotation = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
        Runner.Spawn(_projectilePrefab, _shootPoint.position, Quaternion.Euler(0, 0, _bulletRotation), Object.InputAuthority,
            (runner, spawnedObject) =>
            {
                var bullet = spawnedObject.GetComponent<Projectile>();
                bullet.Initialize(direction, _lifeTime, _damage, _speed);
            });
    }
    
}
