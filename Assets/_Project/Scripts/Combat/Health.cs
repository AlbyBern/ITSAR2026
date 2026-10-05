using System;
using UnityEngine;

/// <summary>
/// Punti vita riutilizzabili da player, nemici e strutture.
/// Health gestisce solo la vita e notifica i cambiamenti: non sa chi lo colpisce né chi lo ascolta.
/// </summary>
public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField, Tooltip("Distrugge il GameObject quando la vita arriva a zero")]
    private bool destroyOnDeath;

    public float Current { get; private set; }
    public float Max => maxHealth;
    public bool IsDead => Current <= 0f;

    /// <summary>Invocato a ogni variazione: (vita attuale, vita massima).</summary>
    public event Action<float, float> HealthChanged;

    /// <summary>Invocato una sola volta, quando la vita arriva a zero.</summary>
    public event Action Died;

    private void Awake()
    {
        Current = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || amount <= 0f) return;

        Current = Mathf.Max(0f, Current - amount);
        HealthChanged?.Invoke(Current, maxHealth);

        if (Current <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f) return;

        Current = Mathf.Min(maxHealth, Current + amount);
        HealthChanged?.Invoke(Current, maxHealth);
    }

    private void Die()
    {
        Died?.Invoke();

        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }
}
