using UnityEngine;

/// <summary>
/// Raggio istantaneo: un Raycast verso il bersaglio colpisce il primo nemico (o si ferma sull'ostacolo).
/// Il LineRenderer mostra il raggio per un istante.
/// </summary>
public class BeamWeapon : Weapon
{
    [SerializeField] private Transform muzzle;
    [SerializeField, Tooltip("Layer colpiti dal raggio: nemici e ostacoli")] private LayerMask hitMask;
    [SerializeField] private LineRenderer beam;
    [SerializeField] private float beamDuration = 0.08f;

    private float beamTimeLeft;

    protected override void Awake()
    {
        base.Awake();
        if (beam != null) beam.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        if (beam != null && beam.enabled)
        {
            beamTimeLeft -= Time.deltaTime;
            if (beamTimeLeft <= 0f) beam.enabled = false;
        }
    }

    protected override void Fire()
    {
        Vector3 origin = muzzle != null ? muzzle.position : transform.position + Vector3.up;
        Vector3 direction = (targeter.TargetPoint - origin).normalized;
        Vector3 end = origin + direction * targeter.Range;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, targeter.Range, hitMask, QueryTriggerInteraction.Collide))
        {
            end = hit.point;
            Health health = hit.collider.GetComponentInParent<Health>();
            if (health != null && !health.IsDead)
            {
                health.TakeDamage(damage);
            }
        }

        if (beam != null)
        {
            beam.SetPosition(0, origin);
            beam.SetPosition(1, end);
            beam.enabled = true;
            beamTimeLeft = beamDuration;
        }
    }
}
