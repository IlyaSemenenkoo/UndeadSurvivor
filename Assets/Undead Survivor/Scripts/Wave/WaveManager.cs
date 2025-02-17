using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class WaveManager : NetworkBehaviour
{
    [SerializeField] private List<WaveSettings> _waves;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject _overlay;
    private List<SpawnJob> _jobs = new List<SpawnJob>();
    private int _currentWave;
    private bool _started;
    private void SyncOverlaysState(bool state)
    {
        _overlay.SetActive(state);
    }
    
    [Networked, OnChangedRender(nameof(SyncTime))]
    public TickTimer TickTimer {get; set; }

    public event Action<int> OnTimeChanged;
    private void SyncTime()
    {
        var time = (int?)TickTimer.RemainingTime(Runner);
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

        TickTimer = TickTimer.CreateFromSeconds(Runner, _waves[_currentWave].WaveTime);
    }

    public void StartSpawn()
    {
        if (HasStateAuthority)
        {
            _started = true;
            StartWave();
            _currentWave++;
            RPC_SyncStartUI(true);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_SyncStartUI(bool state)
    {
        SyncOverlaysState(state);
    }
    
    private void FixedUpdate()
    {
        SyncTime();
        if (!HasStateAuthority)
        {
            return;
        }

        if (!_started)
        {
            return;
        }

        if (TickTimer.Expired(Runner))
        {
            if (_currentWave == 6)
            {
                TickTimer = TickTimer.None;
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