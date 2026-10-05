using UnityEngine;

/// <summary>
/// Gemma raccoglibile. Sta sul layer Pickup, che collide solo con il layer Player:
/// grazie alla Physics Matrix, OnTriggerEnter scatta solo quando la tocca il giocatore.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Gem : MonoBehaviour
{
    [SerializeField] private int value = 1;

    private void OnTriggerEnter(Collider other)
    {
        GemCollector collector = other.GetComponentInParent<GemCollector>();
        if (collector == null) return;

        collector.Collect(value);
        Destroy(gameObject);
    }
}
