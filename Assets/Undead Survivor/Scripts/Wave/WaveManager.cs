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

    [Networked, OnChangedRender(nameof(SyncTime))]
    private TickTimer _tickTimer {get; set; }

    public event Action<float> OnTimeChanged;
    private void SyncTime()
    {
        Debug.Log("SyncTime");
        var time = _tickTimer.RemainingTime(Runner);
        if (time.HasValue)
        {
            OnTimeChanged?.Invoke(time.Value);
        }
    }
    
    

    public void StartWave()
    {
        _jobs.Clear();
        Debug.Log(_currentWave + " current wave");
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
            if (Runner.IsServer)
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
        }
    }
}