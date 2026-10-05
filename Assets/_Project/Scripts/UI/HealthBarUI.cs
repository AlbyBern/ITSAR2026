using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Barra della vita: osserva l'evento HealthChanged di un Health (Observer pattern).
/// Health non conosce la UI: è la UI a iscriversi.
/// </summary>
public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Health target;
    [SerializeField, Tooltip("Image di tipo Filled")] private Image fill;

    private void OnEnable()
    {
        if (target != null) target.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        if (target != null) target.HealthChanged -= OnHealthChanged;
    }

    private void Start()
    {
        // Stato iniziale: l'evento arriva solo alla prima variazione
        if (target != null) OnHealthChanged(target.Current, target.Max);
    }

    private void OnHealthChanged(float current, float max)
    {
        fill.fillAmount = max > 0f ? current / max : 0f;
    }
}
