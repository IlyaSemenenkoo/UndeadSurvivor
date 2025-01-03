using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public float Damage { get; set; }
    private float _lifeTime;
    [SerializeField] private NetworkTransform _networkTransform;
    private Vector2 _direction;
    private int _damage;

    public void Initialize(Vector2 direction, float lifeTime, int damage)
    {
        _lifeTime = lifeTime;
        _direction = direction;
        _damage = damage;
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            if (_lifeTime > 0)
            {
                _networkTransform.transform.position += new Vector3(_direction.x, _direction.y, 0) * Runner.DeltaTime;
            }
            else DeleteBullet();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (HasStateAuthority)
        {
            if (collision.gameObject.TryGetComponent<HealthManager>(out var healthManager))
            {
                healthManager.SubtractHP(_damage);
                DeleteBullet();
            }
        }
    }

    private void DeleteBullet()
    {
        Runner.Despawn(Object);
    }
}
    