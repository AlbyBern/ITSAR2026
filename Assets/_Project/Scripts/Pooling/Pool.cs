using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

/// <summary>
/// Object pooling: invece di creare e distruggere oggetti (Instantiate/Destroy), li ricicla.
/// Un pool per ogni prefab, basato su UnityEngine.Pool.ObjectPool.
///
///   Pool.Spawn(prefab, posizione, rotazione)  al posto di Instantiate
///   Pool.Despawn(oggetto)                     al posto di Destroy
/// </summary>
public static class Pool
{
    private static readonly Dictionary<GameObject, ObjectPool<GameObject>> pools = new Dictionary<GameObject, ObjectPool<GameObject>>();
    private static Transform container;

    public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
    {
        return Spawn(prefab.gameObject, position, rotation).GetComponent<T>();
    }

    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject instance = GetPool(prefab).Get();
        instance.transform.SetPositionAndRotation(position, rotation);
        instance.SetActive(true);
        instance.GetComponent<PooledObject>().NotifySpawned();
        return instance;
    }

    public static void Despawn(GameObject instance)
    {
        // Oggetti non creati dal pool (es. piazzati a mano in scena): distruzione normale
        if (!instance.TryGetComponent(out PooledObject pooled) || pooled.Owner == null)
        {
            Object.Destroy(instance);
            return;
        }

        // Già restituito: restituirlo due volte corromperebbe il pool
        if (!pooled.IsSpawned) return;

        pooled.NotifyDespawned();
        instance.SetActive(false);
        pooled.Owner.Release(instance);
    }

    /// <summary>Crea in anticipo le istanze, per non pagarne il costo durante il gioco.</summary>
    public static void Prewarm(GameObject prefab, int count)
    {
        ObjectPool<GameObject> pool = GetPool(prefab);
        var temp = new List<GameObject>(count);
        for (int i = 0; i < count; i++) temp.Add(pool.Get());
        foreach (GameObject go in temp) pool.Release(go);
    }

    private static ObjectPool<GameObject> GetPool(GameObject prefab)
    {
        if (pools.TryGetValue(prefab, out ObjectPool<GameObject> pool)) return pool;

        if (container == null) container = new GameObject("[Pools]").transform;

        ObjectPool<GameObject> created = null;
        created = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject go = Object.Instantiate(prefab, container);
                go.SetActive(false);
                if (!go.TryGetComponent(out PooledObject pooled)) pooled = go.AddComponent<PooledObject>();
                pooled.Owner = created;
                return go;
            },
            actionOnDestroy: go => Object.Destroy(go),
            collectionCheck: false,
            defaultCapacity: 32,
            maxSize: 2000);

        pools.Add(prefab, created);
        return created;
    }

    // Le istanze appartengono alla scena: quando la scena viene scaricata, i pool vanno dimenticati.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        pools.Clear();
        container = null;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private static void OnSceneUnloaded(Scene scene)
    {
        pools.Clear();
        container = null;
    }
}
