using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponeSettings", menuName = "Weapone Settings", order = 0)]
public class WeaponeSettings : ScriptableObject
{
    [SerializeField] public int Damage;
    [SerializeField] public float LifeTime;
    [SerializeField] public float AttackCulldown;
    [SerializeField] public int MaxAmmoInMagazine;
    [SerializeField] public int MagazineAmount;
    [SerializeField] public float Speed;
    [SerializeField] public Projectile ProjectilePrefab;
}