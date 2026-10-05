using UnityEngine;

/// <summary>
/// Configura un nemico a partire da un EnemyData: un solo prefab, tanti tipi di nemico.
/// Il colore si cambia con un MaterialPropertyBlock, senza creare un materiale per ogni nemico.
/// Il comportamento è affidato alla macchina a stati (EnemyAI).
/// </summary>
[RequireComponent(typeof(Health), typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField] private Renderer bodyRenderer;

    private Health health;
    private EnemyAI ai;
    private DropOnDeath drop;
    private MaterialPropertyBlock propertyBlock;

    public EnemyData Data { get; private set; }

    private void Awake()
    {
        health = GetComponent<Health>();
        ai = GetComponent<EnemyAI>();
        drop = GetComponent<DropOnDeath>();
        propertyBlock = new MaterialPropertyBlock();
    }

    /// <summary>Applica i dati del tipo di nemico. Da chiamare subito dopo lo spawn.</summary>
    public void Init(EnemyData data, Health crystal, Health player)
    {
        Data = data;
        health.SetMax(data.MaxHealth);
        drop.Chance = data.GemDropChance;
        transform.localScale = Vector3.one * data.Scale;

        propertyBlock.SetColor(BaseColorId, data.Color);
        bodyRenderer.SetPropertyBlock(propertyBlock);

        ai.Setup(data, crystal, player);
    }
}
