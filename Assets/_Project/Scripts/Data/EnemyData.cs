using UnityEngine;

/// <summary>Configurazione di un tipo di nemico. Un solo prefab, tanti tipi diversi grazie ai dati.</summary>
[CreateAssetMenu(menuName = "Crystal Survivors/Enemy Data", fileName = "Enemy_")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string displayName = "Nemico";
    [SerializeField, Min(1f)] private float maxHealth = 30f;
    [SerializeField, Min(0f)] private float moveSpeed = 2.5f;
    [SerializeField, Min(0f)] private float contactDamagePerSecond = 10f;
    [SerializeField, Range(0f, 1f)] private float gemDropChance = 1f;
    [SerializeField] private Color color = Color.red;
    [SerializeField, Min(0.1f)] private float scale = 1f;

    public string DisplayName => displayName;
    public float MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public float ContactDamagePerSecond => contactDamagePerSecond;
    public float GemDropChance => gemDropChance;
    public Color Color => color;
    public float Scale => scale;
}
