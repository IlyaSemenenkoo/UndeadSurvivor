using UnityEngine;

public class Shotgun : WeaponeBaseType
{   
    private Quaternion rotation;
    protected override void Shoot(Vector2 direction)
    {
        if(!HasInputAuthority) return;
        
        rotation = Quaternion.AngleAxis(15, Vector3.forward);
        SpawnBullet(rotation * direction);
        
        SpawnBullet(direction);
        
        rotation = Quaternion.AngleAxis(-15, Vector3.forward);
        SpawnBullet(rotation * direction);
    }
}
