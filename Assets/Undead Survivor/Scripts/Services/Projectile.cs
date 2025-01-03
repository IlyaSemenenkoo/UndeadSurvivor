using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public float Damage { get; set; }
    private float _lifeTime;
    [SerializeField] private NetworkRigidbody2D _networkRB;
    private Vector2 _direction;

    public void Initialize(Vector2 direction)
    {
        _direction = direction;
    }
}
    