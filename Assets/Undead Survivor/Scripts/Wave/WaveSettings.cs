using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveSettings", menuName = "Wave Settings", order = 0)]
public class WaveSettings : ScriptableObject
{
    [SerializeField] public List<SpawnData> _SpawnData;
    [SerializeField] public float WaveTime;
}