using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [Fornito dal docente] Infligge danno continuo agli oggetti con Health a contatto con il nemico.
/// </summary>
public class ContactDamage : MonoBehaviour
{
    [SerializeField] private float damagePerSecond = 10f;
    [SerializeField, Tooltip("Layer che subiscono danno (es. Player, Structure)")] private LayerMask targetMask;

    private readonly List<Health> touching = new List<Health>();

    private void OnTriggerEnter(Collider other)
    {
        if ((targetMask.value & (1 << other.gameObject.layer)) == 0) return;

        Health health = other.GetComponentInParent<Health>();
        if (health != null && !touching.Contains(health))
        {
            touching.Add(health);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();
        if (health != null)
        {
            touching.Remove(health);
        }
    }

    private void OnDisable() => touching.Clear();

    private void Update()
    {
        for (int i = touching.Count - 1; i >= 0; i--)
        {
            Health health = touching[i];
            if (health == null || !health.isActiveAndEnabled)
            {
                touching.RemoveAt(i);
                continue;
            }
            health.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
