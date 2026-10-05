using UnityEngine;
using UnityEngine.UI;

/// <summary>Mostra il numero di gemme, aggiornato tramite l'evento GemsChanged.</summary>
public class GemCounterUI : MonoBehaviour
{
    [SerializeField] private GemCollector collector;
    [SerializeField] private Text label;
    [SerializeField] private string format = "Gemme: {0}";

    private void OnEnable()
    {
        if (collector != null) collector.GemsChanged += OnGemsChanged;
    }

    private void OnDisable()
    {
        if (collector != null) collector.GemsChanged -= OnGemsChanged;
    }

    private void Start()
    {
        if (collector != null) OnGemsChanged(collector.Gems);
    }

    private void OnGemsChanged(int gems)
    {
        label.text = string.Format(format, gems);
    }
}
