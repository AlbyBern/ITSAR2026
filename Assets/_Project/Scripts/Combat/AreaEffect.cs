using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Area a trigger che danneggia o cura nel tempo tutto ciò che ha un componente Health.
/// Richiede un Collider con "Is Trigger" attivo.
/// </summary>
[RequireComponent(typeof(Collider))]
public class AreaEffect : MonoBehaviour
{
    public enum EffectType { Damage, Heal }

    [SerializeField] private EffectType effect = EffectType.Damage;
    [SerializeField, Tooltip("Punti vita al secondo")] private float amountPerSecond = 20f;

    // Chi è dentro l'area in questo momento
    private readonly List<Health> inside = new List<Health>();

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();
        if (health != null && !inside.Contains(health))
        {
            inside.Add(health);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();
        if (health != null)
        {
            inside.Remove(health);
        }
    }

    private void Update()
    {
        float amount = amountPerSecond * Time.deltaTime;

        // Ciclo al contrario: possiamo rimuovere elementi senza saltarne nessuno.
        for (int i = inside.Count - 1; i >= 0; i--)
        {
            Health health = inside[i];

            // Un oggetto distrutto o disattivato non chiama OnTriggerExit: lo togliamo noi.
            if (health == null || !health.isActiveAndEnabled)
            {
                inside.RemoveAt(i);
                continue;
            }

            if (effect == EffectType.Damage) health.TakeDamage(amount);
            else health.Heal(amount);
        }
    }
}
