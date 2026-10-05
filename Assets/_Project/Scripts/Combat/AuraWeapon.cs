using UnityEngine;

/// <summary>
/// Aura: a ogni attacco danneggia tutti i nemici entro un raggio. Non ha bisogno di un bersaglio.
/// </summary>
public class AuraWeapon : Weapon
{
    [SerializeField] private float radius = 3f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField, Tooltip("Sfera semitrasparente che mostra il raggio")] private Transform visual;

    protected override bool NeedsTarget => false;

    protected override void Awake()
    {
        base.Awake();
        UpdateVisual();
    }

    protected override void Fire()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyMask, QueryTriggerInteraction.Collide);
        foreach (Collider hit in hits)
        {
            Health health = hit.GetComponentInParent<Health>();
            if (health != null && !health.IsDead)
            {
                health.TakeDamage(damage);
            }
        }
    }

    private void OnEnable()
    {
        if (visual != null) visual.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (visual != null) visual.gameObject.SetActive(false);
    }

    private void UpdateVisual()
    {
        if (visual != null)
        {
            visual.localScale = new Vector3(radius * 2f, 0.05f, radius * 2f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.6f, 0.3f, 1f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
