using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class WaveManager : NetworkBehaviour
{
    [SerializeField] private List<WaveSettings> _waves;
    [SerializeField] private Transform[] _spawnPoints;
    private List<SpawnJob> _jobs;
    private int _currentWave = 0;
    private TickTimer _tickTimer;

    public void StartWave()
    {
        _jobs.Clear();
        foreach (var group in _waves[_currentWave]._SpawnData)
        {
            _jobs.Add(new SpawnJob( group.ObjectPrefab, group.SpawnDelay, group.SpawnCount, _spawnPoints, Runner));
        }
        _tickTimer = TickTimer.CreateFromSeconds(Runner, _waves[_currentWave].WaveTime);
        _currentWave++;
    }

    private void FixedUpdate()
    {
        if (_tickTimer.IsRunning)
        {
            foreach (var job in _jobs)
            {
                job.Tick();
            }
        }
        else
        {
            StartWave();
        }
    }
}
