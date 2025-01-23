using UnityEngine;

public class Rifle : WeaponeBaseType
{
    public override void Shoot(Vector2 direction)
    {
        SpawnBullet(direction);
    }
}
