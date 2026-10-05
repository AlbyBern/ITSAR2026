using UnityEngine;

/// <summary>Spara un proiettile verso il bersaglio (ex SimpleShooter, ora parte della gerarchia Weapon).</summary>
public class ProjectileWeapon : Weapon
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float projectileSpeed = 18f;

    protected override void Fire()
    {
        Vector3 origin = muzzle != null ? muzzle.position : transform.position + Vector3.up;
        Vector3 direction = (targeter.TargetPoint - origin).normalized;

        Projectile projectile = Instantiate(projectilePrefab, origin, Quaternion.LookRotation(direction));
        projectile.Launch(direction * projectileSpeed, damage);
    }
}
