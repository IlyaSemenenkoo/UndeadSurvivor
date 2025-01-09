using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class WaveManager : NetworkBehaviour
{
    [SerializeField] private List<WaveSettings> _waves;
    [SerializeField] private Transform[] _spawnPoints;
    private List<SpawnJob> _jobs = new List<SpawnJob>();
    private int _currentWave;
    private TickTimer _tickTimer;

    public void StartWave()
    {
        _jobs.Clear();
        Debug.Log(_currentWave + " current wave");
        if (_waves[_currentWave]._SpawnData.Count > 0)
        {
            foreach (var group in _waves[_currentWave]._SpawnData)
            {
                _jobs.Add(new SpawnJob( group.ObjectPrefab, group.SpawnDelay, group.SpawnCount, _spawnPoints, Runner));
            }
        }
        _tickTimer = TickTimer.CreateFromSeconds(Runner, _waves[_currentWave].WaveTime);
    }

    private void FixedUpdate()
    {
        if (Runner.IsServer)
        {
            if (_tickTimer.Expired(Runner))
            {
                StartWave();
                _currentWave++;
            }
            else
            {
                foreach (var job in _jobs)
                {
                    job.Tick();
                }
            }
        }
    }
}
