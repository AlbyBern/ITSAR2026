using UnityEngine;

/// <summary>
/// Classe base dei pickup: oggetti che il player raccoglie toccandoli.
/// Ogni sottoclasse decide l'effetto in TryApply; se l'effetto non serve (es. vita piena)
/// il pickup resta a terra.
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class Pickup : MonoBehaviour, IPoolable
{
    private bool consumed;

    /// <summary>Il cristallo dell'arena, per i pickup che agiscono su di esso.</summary>
    protected Health Crystal { get; private set; }

    /// <summary>Chiamato dallo spawner subito dopo lo spawn.</summary>
    public void Init(Health crystal) => Crystal = crystal;

    private void OnTriggerEnter(Collider other)
    {
        // Due trigger nello stesso frame: l'effetto si applica una volta sola
        if (consumed) return;

        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player == null) return;

        if (TryApply(player.gameObject))
        {
            consumed = true;
            Pool.Despawn(gameObject);
        }
    }

    /// <summary>Applica l'effetto. Restituisce true se il pickup va consumato.</summary>
    protected abstract bool TryApply(GameObject player);

    public void OnSpawned() => consumed = false;
    public void OnDespawned() { }
}
