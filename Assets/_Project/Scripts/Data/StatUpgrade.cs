using UnityEngine;

/// <summary>Potenziamento di una statistica del giocatore.</summary>
[CreateAssetMenu(menuName = "Crystal Survivors/Upgrades/Stat Upgrade", fileName = "Upgrade_")]
public class StatUpgrade : UpgradeData
{
    public enum Stat { Damage, FireRate, MoveSpeed, MaxHealth }

    [SerializeField] private Stat stat;
    [SerializeField, Tooltip("Percentuale (0.2 = +20%) o punti vita per MaxHealth")] private float amount = 0.2f;

    public override void Apply(GameObject player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();

        switch (stat)
        {
            case Stat.Damage:
                stats.DamageMultiplier += amount;
                break;
            case Stat.FireRate:
                stats.CooldownMultiplier *= 1f - amount;
                break;
            case Stat.MoveSpeed:
                stats.MoveSpeedMultiplier += amount;
                break;
            case Stat.MaxHealth:
                player.GetComponent<Health>().IncreaseMax(amount);
                break;
        }
    }
}
