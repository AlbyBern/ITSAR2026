using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Fa comparire periodicamente un pickup casuale in un punto valido della NavMesh,
/// a una certa distanza dal cristallo.
/// </summary>
public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private List<Pickup> pickupPrefabs = new List<Pickup>();
    [SerializeField] private Health crystal;
    [SerializeField, Tooltip("Secondi tra due pickup")] private float interval = 15f;
    [SerializeField] private float minRadius = 4f;
    [SerializeField] private float maxRadius = 14f;
    [SerializeField] private int maxActive = 3;
    [SerializeField, Tooltip("Tentativi per trovare un punto valido")] private int attempts = 10;

    private readonly List<Pickup> active = new List<Pickup>();
    private float timer;

    private void Start()
    {
        timer = interval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;
        timer = interval;

        // Conteggio senza allocazioni: ciclo all'indietro, niente LINQ né lambda
        for (int i = active.Count - 1; i >= 0; i--)
        {
            if (!active[i].gameObject.activeInHierarchy) active.RemoveAt(i);
        }
        if (active.Count >= maxActive || pickupPrefabs.Count == 0) return;

        if (TryFindPoint(out Vector3 point))
        {
            Pickup prefab = pickupPrefabs[Random.Range(0, pickupPrefabs.Count)];
            Pickup pickup = Pool.Spawn(prefab, point, Quaternion.identity);
            pickup.Init(crystal);
            active.Add(pickup);
        }
    }

    private bool TryFindPoint(out Vector3 point)
    {
        Vector3 center = crystal.transform.position;
        for (int i = 0; i < attempts; i++)
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minRadius, maxRadius);
            Vector3 candidate = center + new Vector3(direction.x, 0f, direction.y) * distance;

            // Il punto deve stare sulla NavMesh: niente pickup dentro le rocce o fuori dall'arena
            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                point = hit.position;
                return true;
            }
        }
        point = Vector3.zero;
        return false;
    }
}
