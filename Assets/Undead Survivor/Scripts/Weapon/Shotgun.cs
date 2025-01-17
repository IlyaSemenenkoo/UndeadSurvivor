using UnityEngine;

public class Shotgun : WeaponeBaseType
{   
    private Quaternion rotation;
    private float _angle = 15;
    public override void Shoot(Vector2 direction)
    {
        if(!HasInputAuthority) return;
        
        rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
        SpawnBullet(rotation * direction);
        
        SpawnBullet(direction);
        
        rotation = Quaternion.AngleAxis(-_angle, Vector3.forward);
        SpawnBullet(rotation * direction);
    }
}
