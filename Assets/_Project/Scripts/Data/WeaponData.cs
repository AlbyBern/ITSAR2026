using UnityEngine;

/// <summary>
/// Configurazione di un'arma. Contiene solo dati stabili: lo stato a runtime
/// (cooldown in corso, potenziamenti presi) resta nei componenti sugli oggetti.
/// </summary>
[CreateAssetMenu(menuName = "Crystal Survivors/Weapon Data", fileName = "Weapon_")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private string displayName = "Arma";
    [SerializeField, Min(0f)] private float damage = 10f;
    [SerializeField, Min(0.05f), Tooltip("Secondi tra due attacchi")] private float cooldown = 1f;
    [SerializeField, Tooltip("Solo armi a proiettile")] private float projectileSpeed = 18f;
    [SerializeField, Tooltip("Solo armi ad area")] private float radius = 3f;

    public string DisplayName => displayName;
    public float Damage => damage;
    public float Cooldown => cooldown;
    public float ProjectileSpeed => projectileSpeed;
    public float Radius => radius;
}
