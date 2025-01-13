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
    private bool _started;

    [Networked]
    private TickTimer _tickTimer {get; set; }

    public event Action<int> OnTimeChanged;
    private void SyncTime()
    {
        var time = (int?)_tickTimer.RemainingTime(Runner);
        if (time.HasValue)
        {
            OnTimeChanged?.Invoke(time.Value);
        }
    }
    
    

    public void StartWave()
    {
        _jobs.Clear();
        if (_waves[_currentWave]._SpawnData.Count > 0)
        {
            foreach (var group in _waves[_currentWave]._SpawnData)
            {
                _jobs.Add(new SpawnJob(group.ObjectPrefab, group.SpawnDelay, group.SpawnCount, _spawnPoints, Runner));
            }
        }

        _tickTimer = TickTimer.CreateFromSeconds(Runner, _waves[_currentWave].WaveTime);
    }

    public void StartSpawn()
    {
        _started = true;
        StartWave();
        _currentWave++;
    }    
    private void FixedUpdate()
    {
        if (_started)
        {
            if (HasStateAuthority)
            {
                if (_tickTimer.Expired(Runner))
                {
                    if (_currentWave == 6)
                    {
                        _tickTimer = TickTimer.None;
                        _currentWave = 0;
                        _started = false;
                    }
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
            SyncTime();
        }
    }
}