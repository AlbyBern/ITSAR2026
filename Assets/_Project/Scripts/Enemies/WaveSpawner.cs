using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Genera i nemici seguendo una sequenza di ondate (WaveData).
/// Il bilanciamento si fa modificando gli asset, non questo script. L'ultima ondata non finisce mai.
/// </summary>
public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField, Tooltip("Bersaglio dei nemici (il cristallo)")] private Transform target;
    [SerializeField] private List<WaveData> waves = new List<WaveData>();

    private readonly List<Enemy> alive = new List<Enemy>();
    private WaveData current;
    private float waveTimeLeft;
    private float spawnTimer;

    /// <summary>Numero dell'ondata corrente, da 1.</summary>
    public int WaveNumber { get; private set; }

    /// <summary>(numero dell'ondata, dati dell'ondata)</summary>
    public event Action<int, WaveData> WaveStarted;

    private void Start()
    {
        StartWave(0);
    }

    private void Update()
    {
        if (current == null) return;

        waveTimeLeft -= Time.deltaTime;
        if (waveTimeLeft <= 0f && WaveNumber < waves.Count)
        {
            StartWave(WaveNumber);
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer > 0f) return;
        spawnTimer = current.SpawnInterval;

        alive.RemoveAll(enemy => enemy == null);
        if (alive.Count >= current.MaxAlive) return;

        Spawn(current.PickEnemy());
    }

    private void StartWave(int index)
    {
        current = waves[index];
        WaveNumber = index + 1;
        waveTimeLeft = current.Duration;
        WaveStarted?.Invoke(WaveNumber, current);
    }

    private void Spawn(EnemyData data)
    {
        if (data == null) return;

        Transform point = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        Enemy enemy = Instantiate(enemyPrefab, point.position, Quaternion.identity);
        enemy.Init(data, target);
        alive.Add(enemy);
    }
}
