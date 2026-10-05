using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Aggiunto automaticamente agli oggetti creati da Pool: ricorda a quale pool appartiene
/// e avvisa i componenti IPoolable quando l'oggetto viene preso o restituito.
/// </summary>
[DisallowMultipleComponent]
public class PooledObject : MonoBehaviour
{
    private IPoolable[] poolables;

    public ObjectPool<GameObject> Owner { get; set; }
    public bool IsSpawned { get; private set; }

    private void Awake()
    {
        // Cercati una volta sola: a ogni spawn si scorre l'array senza allocare
        poolables = GetComponentsInChildren<IPoolable>(true);
    }

    public void NotifySpawned()
    {
        IsSpawned = true;
        foreach (IPoolable p in poolables) p.OnSpawned();
    }

    public void NotifyDespawned()
    {
        IsSpawned = false;
        foreach (IPoolable p in poolables) p.OnDespawned();
    }
}
