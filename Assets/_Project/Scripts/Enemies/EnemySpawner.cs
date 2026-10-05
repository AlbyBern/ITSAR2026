using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [Fornito dal docente] Genera un nemico a intervalli regolari in un punto di spawn casuale.
/// Nella lezione 06 lascerà il posto a WaveSpawner, guidato da dati (ScriptableObject).
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField, Tooltip("Bersaglio dei nemici (il cristallo)")] private Transform target;
    [SerializeField] private float interval = 1.5f;
    [SerializeField] private int maxAlive = 25;

    private readonly List<GameObject> alive = new List<GameObject>();
    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;
        timer = interval;

        alive.RemoveAll(enemy => enemy == null);
        if (alive.Count >= maxAlive) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, point.position, Quaternion.identity);

        if (enemy.TryGetComponent(out SimpleChaser chaser))
        {
            chaser.SetTarget(target);
        }
        alive.Add(enemy);
    }
}
