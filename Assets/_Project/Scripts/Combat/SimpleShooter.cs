using UnityEngine;

/// <summary>
/// Prima arma del giocatore: spara un proiettile verso il bersaglio trovato da AutoTargeter.
/// (Nella lezione 05 diventerà una delle armi della gerarchia Weapon.)
/// </summary>
public class SimpleShooter : MonoBehaviour
{
    [SerializeField] private AutoTargeter targeter;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform muzzle;
    [SerializeField, Tooltip("Secondi tra due colpi")] private float fireInterval = 0.6f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float projectileSpeed = 18f;

    private float cooldown;

    private void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown > 0f || targeter.CurrentTarget == null) return;

        cooldown = fireInterval;

        Vector3 origin = muzzle != null ? muzzle.position : transform.position + Vector3.up;
        Vector3 direction = (targeter.TargetPoint - origin).normalized;

        Projectile projectile = Instantiate(projectilePrefab, origin, Quaternion.LookRotation(direction));
        projectile.Launch(direction * projectileSpeed, damage);
    }
}
