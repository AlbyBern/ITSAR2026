using System;
using UnityEngine;

/// <summary>
/// Conta le gemme raccolte dal giocatore. Le gemme sono anche la valuta per costruire le torri.
/// </summary>
public class GemCollector : MonoBehaviour
{
    public int Gems { get; private set; }

    /// <summary>Totale aggiornato delle gemme.</summary>
    public event Action<int> GemsChanged;

    /// <summary>Quantità appena raccolta (non spesa).</summary>
    public event Action<int> GemsCollected;

    public void Collect(int amount)
    {
        Gems += amount;
        GemsCollected?.Invoke(amount);
        GemsChanged?.Invoke(Gems);
    }

    public bool TrySpend(int cost)
    {
        if (Gems < cost) return false;

        Gems -= cost;
        GemsChanged?.Invoke(Gems);
        return true;
    }
}
