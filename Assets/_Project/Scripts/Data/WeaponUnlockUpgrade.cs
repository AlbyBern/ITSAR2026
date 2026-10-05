using UnityEngine;

/// <summary>Sblocca un'arma del giocatore: attiva il componente Weapon che usa quei dati.</summary>
[CreateAssetMenu(menuName = "Crystal Survivors/Upgrades/Weapon Unlock", fileName = "Upgrade_Unlock")]
public class WeaponUnlockUpgrade : UpgradeData
{
    [SerializeField] private WeaponData weapon;

    public override bool IsAvailable(GameObject player)
    {
        Weapon w = Find(player);
        return w != null && !w.enabled;
    }

    public override void Apply(GameObject player)
    {
        Weapon w = Find(player);
        if (w != null) w.enabled = true;
    }

    private Weapon Find(GameObject player)
    {
        // true = includi i componenti disattivati
        foreach (Weapon w in player.GetComponentsInChildren<Weapon>(true))
        {
            if (w.Data == weapon) return w;
        }
        return null;
    }
}
