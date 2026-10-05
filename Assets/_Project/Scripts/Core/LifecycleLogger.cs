using UnityEngine;

/// <summary>
/// Scrive in Console ogni metodo del ciclo di vita del MonoBehaviour,
/// per osservare in che ordine Unity li chiama.
/// </summary>
public class LifecycleLogger : MonoBehaviour
{
    [SerializeField] private bool logUpdate = true;

    private bool firstUpdateLogged;

    private void Awake() => Log("Awake");
    private void OnEnable() => Log("OnEnable");
    private void Start() => Log("Start");

    private void Update()
    {
        // Update viene chiamato ogni frame: lo registriamo solo la prima volta.
        if (!logUpdate || firstUpdateLogged) return;
        firstUpdateLogged = true;
        Log("Update (primo frame)");
    }

    private void OnDisable() => Log("OnDisable");
    private void OnDestroy() => Log("OnDestroy");

    private void Log(string method)
    {
        Debug.Log($"[{Time.frameCount}] {name} → {method}", this);
    }
}
