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
    private float _speed;

    public void Initialize(Vector2 direction, float lifeTime, int damage, float speed)
    {
        _lifeTime = lifeTime;
        _direction = direction;
        _damage = damage;
        _speed = speed;
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            if (_lifeTime > 0)
            {
                _networkTransform.transform.position += new Vector3(_direction.x, _direction.y, 0) * Runner.DeltaTime * _speed;
                _lifeTime -= Runner.DeltaTime;
            }
            else DeleteBullet();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (HasStateAuthority)
        {
            if (collision.gameObject.TryGetComponent(out EnemyHealthManager enemyHealthManager))
            {
                var input = this.Object.InputAuthority;
                DeleteBullet();
                enemyHealthManager.SubtractHP(_damage,input);
                
            }
        }
    }

    private void DeleteBullet()
    {
        Runner.Despawn(Object);
    }
}
    