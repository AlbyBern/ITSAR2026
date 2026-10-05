using UnityEngine;

/// <summary>
/// Quando l'Health di questo oggetto muore, lascia cadere un oggetto (una gemma).
/// È un esempio di Observer: si iscrive all'evento Died senza che Health sappia della sua esistenza.
/// </summary>
[RequireComponent(typeof(Health))]
public class DropOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab;
    [SerializeField, Range(0f, 1f)] private float chance = 1f;

    private Health health;

    public float Chance
    {
        get => chance;
        set => chance = Mathf.Clamp01(value);
    }

    private void Awake() => health = GetComponent<Health>();
    private void OnEnable() => health.Died += OnDied;
    private void OnDisable() => health.Died -= OnDied;

    private void OnDied()
    {
        if (dropPrefab != null && Random.value <= chance)
        {
            Pool.Spawn(dropPrefab, transform.position, Quaternion.identity);
        }
    }
}
