using UnityEngine;

/// <summary>Configurazione di un tipo di nemico. Un solo prefab, tanti tipi diversi grazie ai dati.</summary>
[CreateAssetMenu(menuName = "Crystal Survivors/Enemy Data", fileName = "Enemy_")]
public class EnemyData : ScriptableObject
{
    public enum TargetPriority { Crystal, Player, Towers }

    [SerializeField] private string displayName = "Nemico";
    [SerializeField, Min(1f)] private float maxHealth = 30f;
    [SerializeField, Min(0f)] private float moveSpeed = 2.5f;
    [SerializeField, Min(0f)] private float contactDamagePerSecond = 10f;
    [SerializeField, Range(0f, 1f)] private float gemDropChance = 1f;
    [SerializeField] private Color color = Color.red;
    [SerializeField, Min(0.1f)] private float scale = 1f;

    [Header("Intelligenza artificiale")]
    [SerializeField, Tooltip("Bersaglio principale")] private TargetPriority priority = TargetPriority.Crystal;
    [SerializeField, Tooltip("Entro questa distanza il nemico insegue il player")] private float aggroRange = 5f;
    [SerializeField, Tooltip("Distanza dalla superficie del bersaglio per attaccare")] private float attackRange = 1.2f;
    [SerializeField, Min(0.1f), Tooltip("Secondi tra due colpi")] private float attackInterval = 0.8f;

    public string DisplayName => displayName;
    public float MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public float ContactDamagePerSecond => contactDamagePerSecond;
    public float GemDropChance => gemDropChance;
    public Color Color => color;
    public float Scale => scale;
    public TargetPriority Priority => priority;
    public float AggroRange => aggroRange;
    public float AttackRange => attackRange;
    public float AttackInterval => attackInterval;
}
