using UnityEngine;

/// <summary>
/// Configura un nemico a partire da un EnemyData: un solo prefab, tanti tipi di nemico.
/// Il colore si cambia con un MaterialPropertyBlock, senza creare un materiale per ogni nemico.
/// </summary>
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField] private Renderer bodyRenderer;

    private Health health;
    private SimpleChaser chaser;
    private ContactDamage contactDamage;
    private DropOnDeath drop;
    private MaterialPropertyBlock propertyBlock;

    public EnemyData Data { get; private set; }

    private void Awake()
    {
        health = GetComponent<Health>();
        chaser = GetComponent<SimpleChaser>();
        contactDamage = GetComponent<ContactDamage>();
        drop = GetComponent<DropOnDeath>();
        propertyBlock = new MaterialPropertyBlock();
    }

    /// <summary>Applica i dati del tipo di nemico. Da chiamare subito dopo la creazione.</summary>
    public void Init(EnemyData data, Transform target)
    {
        Data = data;
        health.SetMax(data.MaxHealth);
        chaser.SetTarget(target);
        chaser.Speed = data.MoveSpeed;
        contactDamage.DamagePerSecond = data.ContactDamagePerSecond;
        drop.Chance = data.GemDropChance;
        transform.localScale = Vector3.one * data.Scale;

        propertyBlock.SetColor(BaseColorId, data.Color);
        bodyRenderer.SetPropertyBlock(propertyBlock);
    }
}
