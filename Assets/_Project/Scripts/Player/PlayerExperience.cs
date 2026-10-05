using System;
using UnityEngine;

/// <summary>
/// Esperienza e livello. Ascolta l'evento GemsCollected di GemCollector (Observer):
/// ogni gemma raccolta vale esperienza, senza che GemCollector sappia dell'esperienza.
/// </summary>
[RequireComponent(typeof(GemCollector))]
public class PlayerExperience : MonoBehaviour
{
    [SerializeField, Tooltip("Esperienza per passare dal livello 1 al 2")] private int baseRequirement = 5;
    [SerializeField, Tooltip("Esperienza in più richiesta a ogni livello")] private int growthPerLevel = 4;

    private GemCollector gems;

    public int Level { get; private set; } = 1;
    public int Experience { get; private set; }
    public int Required => baseRequirement + (Level - 1) * growthPerLevel;

    /// <summary>(esperienza attuale, esperienza richiesta)</summary>
    public event Action<int, int> ExperienceChanged;

    /// <summary>Nuovo livello raggiunto.</summary>
    public event Action<int> LeveledUp;

    private void Awake() => gems = GetComponent<GemCollector>();
    private void OnEnable() => gems.GemsCollected += AddExperience;
    private void OnDisable() => gems.GemsCollected -= AddExperience;

    public void AddExperience(int amount)
    {
        Experience += amount;

        while (Experience >= Required)
        {
            Experience -= Required;
            Level++;
            LeveledUp?.Invoke(Level);
        }

        ExperienceChanged?.Invoke(Experience, Required);
    }
}
