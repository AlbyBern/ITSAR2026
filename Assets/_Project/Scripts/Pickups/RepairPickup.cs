using UnityEngine;

/// <summary>Ripara il cristallo. Se il cristallo è intatto non viene consumato.</summary>
public class RepairPickup : Pickup
{
    [SerializeField] private float repairAmount = 80f;

    protected override bool TryApply(GameObject player)
    {
        if (Crystal == null || Crystal.IsDead || Crystal.Current >= Crystal.Max) return false;

        Crystal.Heal(repairAmount);
        return true;
    }
}
