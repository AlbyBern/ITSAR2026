using UnityEngine;

/// <summary>
/// Trova automaticamente il bersaglio più vicino entro un raggio:
/// 1. OverlapSphere sul layer dei bersagli (query fisica volumetrica)
/// 2. Linecast sul layer degli ostacoli per scartare chi è coperto (linea di tiro)
/// </summary>
public class AutoTargeter : MonoBehaviour
{
    [SerializeField] private float range = 9f;
    [SerializeField, Tooltip("Layer dei bersagli (es. Enemy)")] private LayerMask targetMask;
    [SerializeField, Tooltip("Layer che bloccano la linea di tiro (es. Obstacle)")] private LayerMask obstacleMask;
    [SerializeField, Tooltip("Secondi tra due ricerche: non serve cercare a ogni frame")] private float scanInterval = 0.2f;
    [SerializeField, Tooltip("Altezza da cui si controlla la linea di tiro")] private float eyeHeight = 1f;

    private float nextScanTime;

    public Health CurrentTarget { get; private set; }

    /// <summary>Punto da mirare: il centro del bersaglio, a metà altezza.</summary>
    public Vector3 TargetPoint => CurrentTarget != null ? CurrentTarget.transform.position + Vector3.up * eyeHeight : transform.position;

    public float Range
    {
        get => range;
        set => range = value;
    }

    private void Update()
    {
        // Il bersaglio corrente può essere morto o uscito dal raggio nel frattempo
        if (CurrentTarget != null && !IsStillValid(CurrentTarget))
        {
            CurrentTarget = null;
        }

        if (Time.time < nextScanTime) return;
        nextScanTime = Time.time + scanInterval;
        CurrentTarget = FindNearest();
    }

    private Health FindNearest()
    {
        Vector3 eye = transform.position + Vector3.up * eyeHeight;
        Collider[] hits = Physics.OverlapSphere(transform.position, range, targetMask, QueryTriggerInteraction.Collide);

        Health best = null;
        float bestSqrDistance = float.MaxValue;

        foreach (Collider hit in hits)
        {
            Health health = hit.GetComponentInParent<Health>();
            if (health == null || health.IsDead) continue;

            Vector3 point = health.transform.position + Vector3.up * eyeHeight;
            float sqrDistance = (point - eye).sqrMagnitude;
            if (sqrDistance >= bestSqrDistance) continue;

            // Linea di tiro: se un ostacolo è in mezzo, il bersaglio non è valido
            if (Physics.Linecast(eye, point, obstacleMask, QueryTriggerInteraction.Ignore)) continue;

            best = health;
            bestSqrDistance = sqrDistance;
        }

        return best;
    }

    private bool IsStillValid(Health target)
    {
        if (target.IsDead || !target.gameObject.activeInHierarchy) return false;
        return (target.transform.position - transform.position).sqrMagnitude <= range * range;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.8f, 0.2f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, range);

        if (CurrentTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up * eyeHeight, TargetPoint);
        }
    }
}
