using UnityEngine;

public class Rifle : WeaponeBaseType
{
    protected override void Shoot(Vector2 direction)
    {
        Debug.Log("Shoot Rifle");
        SpawnBullet(direction);
    }
}
