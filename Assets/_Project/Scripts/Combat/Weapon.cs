using UnityEngine;

/// <summary>
/// Classe base di tutte le armi. Contiene la logica comune (cooldown, bersaglio)
/// e lascia alle classi figlie solo il "come" si colpisce: il metodo astratto Fire.
/// Player e torri usano le stesse armi: è la composizione a decidere chi spara.
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    [SerializeField, Tooltip("Se vuoto lo cerca sull'oggetto o sui genitori")] protected AutoTargeter targeter;
    [SerializeField] protected float damage = 10f;
    [SerializeField, Tooltip("Secondi tra due attacchi")] protected float cooldown = 1f;

    private float timer;

    /// <summary>Le armi ad area colpiscono anche senza un bersaglio preciso.</summary>
    protected virtual bool NeedsTarget => true;

    protected virtual void Awake()
    {
        if (targeter == null)
        {
            targeter = GetComponentInParent<AutoTargeter>();
        }
    }

    protected virtual void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;
        if (NeedsTarget && targeter.CurrentTarget == null) return;

        timer = cooldown;
        Fire();
    }

    /// <summary>Esegue un attacco. Ogni arma lo implementa a modo suo.</summary>
    protected abstract void Fire();
}
