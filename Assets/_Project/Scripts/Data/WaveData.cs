using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Descrive un'ondata: quanto dura, ogni quanto nasce un nemico e quali nemici.</summary>
[CreateAssetMenu(menuName = "Crystal Survivors/Wave Data", fileName = "Wave_")]
public class WaveData : ScriptableObject
{
    [Serializable]
    public class Entry
    {
        public EnemyData enemy;
        [Min(1)] public int weight = 1;
    }

    [SerializeField, Tooltip("Durata in secondi")] private float duration = 40f;
    [SerializeField, Tooltip("Secondi tra due nemici")] private float spawnInterval = 1.5f;
    [SerializeField, Tooltip("Nemici contemporanei al massimo")] private int maxAlive = 30;
    [SerializeField] private List<Entry> enemies = new List<Entry>();

    public float Duration => duration;
    public float SpawnInterval => spawnInterval;
    public int MaxAlive => maxAlive;

    /// <summary>Sceglie un nemico a caso, in proporzione ai pesi.</summary>
    public EnemyData PickEnemy()
    {
        int total = 0;
        foreach (Entry e in enemies) total += e.weight;

        int roll = UnityEngine.Random.Range(0, total);
        foreach (Entry e in enemies)
        {
            if (roll < e.weight) return e.enemy;
            roll -= e.weight;
        }
        return enemies.Count > 0 ? enemies[0].enemy : null;
    }
}
