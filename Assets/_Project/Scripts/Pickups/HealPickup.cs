using UnityEngine;

/// <summary>Cura il player. Con la vita piena non viene consumato.</summary>
public class HealPickup : Pickup
{
    [SerializeField] private float healAmount = 30f;

    protected override bool TryApply(GameObject player)
    {
        Health health = player.GetComponent<Health>();
        if (health == null || health.IsDead || health.Current >= health.Max) return false;

        health.Heal(healAmount);
        return true;
    }
}
