using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class WaveManager : NetworkBehaviour
{
    [SerializeField] private List<WaveSettings> _waves;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject _overlay;
    [SerializeField] private GameObject _uiManager;
    private List<SpawnJob> _jobs = new List<SpawnJob>();
    private int _currentWave;
    private bool _started;

    private int _lastWave = 6;
    private void SyncOverlaysState(bool state)
    {
        _overlay.SetActive(state);
        _uiManager.SetActive(state);
    }
    
    [Networked, OnChangedRender(nameof(SyncTime))] private TickTimer TickTimer {get; set; }

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
        if (_waves[_currentWave].SpawnData.Count > 0)
        {
            foreach (var group in _waves[_currentWave].SpawnData)
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

    public override void FixedUpdateNetwork()
    {
        RPC_SyncTime();
        if (!HasStateAuthority) return;
        if (!_started) return;

        if (TickTimer.Expired(Runner))
        {
            if (_currentWave == _lastWave)
            {
                TickTimer = TickTimer.None;
                _currentWave = 0;
                _started = false;
                PlayerDataSystem._singleton.TimeEnded();
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
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_SyncTime()
    {
        SyncTime();
    }
}