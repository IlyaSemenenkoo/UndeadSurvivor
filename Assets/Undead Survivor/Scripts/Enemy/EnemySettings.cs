using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemySettings", menuName = "Enemy Settings", order = 0)]
public class EnemySettings : ScriptableObject
{
    [SerializeField] public float Speed;
    [SerializeField] public float DetectionRadius;
    [SerializeField] public float AttackRadius;
    [SerializeField] public float AttackCulldown;
    [SerializeField] public int AttackDamage;
}