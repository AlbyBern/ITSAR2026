using UnityEngine;

/// <summary>
/// Classe base di tutte le armi. I valori (danno, cooldown…) arrivano da un WeaponData:
/// cambiare arma o bilanciarla significa cambiare un asset, non il codice.
/// I moltiplicatori del giocatore (PlayerStats) sono stato runtime e restano fuori dall'asset.
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData data;
    [SerializeField, Tooltip("Se vuoto lo cerca sull'oggetto o sui genitori")] protected AutoTargeter targeter;

    private PlayerStats stats;
    private float timer;

    public WeaponData Data => data;

    protected float Damage => data.Damage * (stats != null ? stats.DamageMultiplier : 1f);
    protected float Cooldown => data.Cooldown * (stats != null ? stats.CooldownMultiplier : 1f);

    /// <summary>Le armi ad area colpiscono anche senza un bersaglio preciso.</summary>
    protected virtual bool NeedsTarget => true;

    protected virtual void Awake()
    {
        if (targeter == null)
        {
            targeter = GetComponentInParent<AutoTargeter>();
        }
        // Le torri non hanno PlayerStats: in quel caso i moltiplicatori valgono 1
        stats = GetComponentInParent<PlayerStats>();
    }

    protected virtual void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;
        if (NeedsTarget && targeter.CurrentTarget == null) return;

        timer = Cooldown;
        Fire();
    }

    /// <summary>Esegue un attacco. Ogni arma lo implementa a modo suo.</summary>
    protected abstract void Fire();
}
