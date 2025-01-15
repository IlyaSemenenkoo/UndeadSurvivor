using Fusion;
using UnityEngine;

public class SpawnJob
{
    public NetworkPrefabRef ObjectPrefab;
    public float SpawnDelay;
    public float SpawnCount;
    public Transform[] SpawnPoints;
    public NetworkRunner Runner;

    public float SpawnTimer;
    
    public SpawnJob(NetworkPrefabRef objectPrefab, float spawnDelay, float spawnCount, Transform[] objectSpawnPoints, NetworkRunner runner)
    {
        ObjectPrefab = objectPrefab;
        SpawnDelay = spawnDelay;
        SpawnCount = spawnCount;
        SpawnPoints = objectSpawnPoints;
        SpawnTimer = SpawnDelay;
        Runner = runner;
    }

    public void Tick()
    {
        if (!Runner.IsServer)
        {
            return;
        }
        if (SpawnTimer <= 0 && SpawnCount != 0)
        {
            int pos = Random.Range(0, SpawnPoints.Length);
            Runner.Spawn(ObjectPrefab, SpawnPoints[pos].position, Quaternion.identity);
            Debug.Log("Spawned at " + SpawnPoints[pos].position);
            SpawnTimer = SpawnDelay;
            SpawnCount--;
        }
        else
        {
            SpawnTimer -= Time.deltaTime;
        }
    }
}
