using UnityEngine;

/// <summary>Spara un proiettile verso il bersaglio.</summary>
public class ProjectileWeapon : Weapon
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform muzzle;

    protected override void Fire()
    {
        Vector3 origin = muzzle != null ? muzzle.position : transform.position + Vector3.up;
        Vector3 direction = (targeter.TargetPoint - origin).normalized;

        Projectile projectile = Pool.Spawn(projectilePrefab, origin, Quaternion.LookRotation(direction));
        projectile.Launch(direction * data.ProjectileSpeed, Damage);
    }
}
