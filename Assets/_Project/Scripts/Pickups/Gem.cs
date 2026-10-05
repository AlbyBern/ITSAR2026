using UnityEngine;

/// <summary>
/// Gemma raccoglibile. Sta sul layer Pickup, che collide solo con il layer Player:
/// grazie alla Physics Matrix, OnTriggerEnter scatta solo quando la tocca il giocatore.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Gem : MonoBehaviour, IPoolable
{
    [SerializeField] private int value = 1;

    private bool collected;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        GemCollector collector = other.GetComponentInParent<GemCollector>();
        if (collector == null) return;

        collected = true;
        collector.Collect(value);
        Pool.Despawn(gameObject);
    }

    public void OnSpawned() => collected = false;
    public void OnDespawned() { }
}
