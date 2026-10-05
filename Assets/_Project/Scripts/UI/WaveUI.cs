using UnityEngine;
using UnityEngine.UI;

/// <summary>Mostra il numero dell'ondata corrente.</summary>
public class WaveUI : MonoBehaviour
{
    [SerializeField] private WaveSpawner spawner;
    [SerializeField] private Text label;

    private void OnEnable() => spawner.WaveStarted += OnWaveStarted;
    private void OnDisable() => spawner.WaveStarted -= OnWaveStarted;

    private void OnWaveStarted(int number, WaveData wave)
    {
        label.text = "Ondata " + number;
    }
}
