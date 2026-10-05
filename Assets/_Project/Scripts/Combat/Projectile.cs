using UnityEngine;

/// <summary>
/// Proiettile con Rigidbody cinematico e collider trigger.
/// Si muove in FixedUpdate con MovePosition: così la fisica rileva correttamente i trigger.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    [SerializeField, Tooltip("Layer che subiscono danno")] private LayerMask damageMask;

    private Rigidbody body;
    private Vector3 velocity;
    private float damage;
    private float timeLeft;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.isKinematic = true;
        body.useGravity = false;
    }

    /// <summary>Da chiamare subito dopo la creazione.</summary>
    public void Launch(Vector3 newVelocity, float newDamage)
    {
        velocity = newVelocity;
        damage = newDamage;
        timeLeft = lifetime;
        transform.rotation = Quaternion.LookRotation(newVelocity);
    }

    private void FixedUpdate()
    {
        body.MovePosition(body.position + velocity * Time.fixedDeltaTime);

        timeLeft -= Time.fixedDeltaTime;
        if (timeLeft <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Confronto con la LayerMask: il bit del layer dell'altro oggetto è acceso?
        bool damageable = (damageMask.value & (1 << other.gameObject.layer)) != 0;
        if (damageable)
        {
            Health health = other.GetComponentInParent<Health>();
            if (health != null && !health.IsDead)
            {
                health.TakeDamage(damage);
            }
        }

        // Si distrugge su qualunque cosa la Physics Matrix gli permetta di toccare (nemici e ostacoli)
        Destroy(gameObject);
    }
}
