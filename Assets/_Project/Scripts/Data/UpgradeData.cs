using UnityEngine;

/// <summary>
/// Potenziamento scelto al passaggio di livello. Classe astratta: è lo Scriptable Object
/// "livello avanzato", cioè un comportamento (strategia) intercambiabile salvato come asset.
/// </summary>
public abstract class UpgradeData : ScriptableObject
{
    [SerializeField] private string title = "Potenziamento";
    [SerializeField, TextArea] private string description = "";

    public string Title => title;
    public string Description => description;

    /// <summary>Se false, il potenziamento non viene proposto (es. arma già sbloccata).</summary>
    public virtual bool IsAvailable(GameObject player) => true;

    /// <summary>Applica l'effetto. Modifica lo stato runtime del player, MAI questo asset.</summary>
    public abstract void Apply(GameObject player);
}
