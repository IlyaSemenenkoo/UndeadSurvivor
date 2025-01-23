using UnityEngine;

public class Shotgun : WeaponeBaseType
{   
    private Quaternion _rotation;
    private float _angle = 15;
    private int _bulletCount = 3;
    public override void Shoot(Vector2 direction)
    {
        for (int i = 0; i < _bulletCount; i++)
        {
            float angle = -15 + i * 15;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            SpawnBullet(rotation * direction);
        }
    }
}
