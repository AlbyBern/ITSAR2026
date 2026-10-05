using UnityEngine;

/// <summary>
/// Aura: a ogni attacco danneggia tutti i nemici entro il raggio (WeaponData.Radius).
/// Non ha bisogno di un bersaglio.
/// </summary>
public class AuraWeapon : Weapon
{
    [SerializeField] private LayerMask enemyMask;
    [SerializeField, Tooltip("Disco che mostra il raggio")] private Transform visual;

    private readonly Collider[] hitBuffer = new Collider[128];

    protected override bool NeedsTarget => false;

    protected override void Fire()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, data.Radius, hitBuffer, enemyMask, QueryTriggerInteraction.Collide);
        for (int i = 0; i < count; i++)
        {
            Health health = hitBuffer[i].GetComponentInParent<Health>();
            if (health != null && !health.IsDead)
            {
                health.TakeDamage(Damage);
            }
        }
    }

    private void OnEnable()
    {
        if (visual == null) return;
        visual.gameObject.SetActive(true);
        visual.localScale = new Vector3(data.Radius * 2f, 0.05f, data.Radius * 2f);
    }

    private void OnDisable()
    {
        if (visual != null) visual.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (data == null) return;
        Gizmos.color = new Color(0.6f, 0.3f, 1f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, data.Radius);
    }
}
