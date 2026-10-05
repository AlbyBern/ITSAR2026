using UnityEngine;

/// <summary>
/// Moltiplicatori del giocatore modificati dai potenziamenti.
/// È STATO RUNTIME: vive sul GameObject e si azzera a ogni partita.
/// Se fosse in uno ScriptableObject, i potenziamenti resterebbero salvati nell'asset.
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public float DamageMultiplier { get; set; } = 1f;
    public float CooldownMultiplier { get; set; } = 1f;
    public float MoveSpeedMultiplier { get; set; } = 1f;
}
