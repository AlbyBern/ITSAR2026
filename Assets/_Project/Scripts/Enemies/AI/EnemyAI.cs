using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controller della macchina a stati del nemico: possiede lo stato corrente e gestisce i passaggi.
/// Il movimento usa NavMeshAgent, quindi i nemici aggirano ostacoli e torri.
///
///   SEEK  ──player vicino──▶ CHASE ──a portata──▶ ATTACK
///    ▲  └──────a portata del bersaglio──────────▶   │
///    └──────────── bersaglio perso / morto ◀────────┘
///   (qualunque stato) ──vita a zero──▶ DEAD
/// </summary>
[RequireComponent(typeof(NavMeshAgent), typeof(Health))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField, Tooltip("Layer delle torri, per i nemici che le attaccano")] private LayerMask structureMask;
    [SerializeField, Tooltip("Secondi tra due ricalcoli del percorso")] private float repathInterval = 0.25f;

    private EnemyState current;

    public NavMeshAgent Agent { get; private set; }
    public Health Health { get; private set; }
    public EnemyData Data { get; private set; }
    public Health Crystal { get; private set; }
    public Health Player { get; private set; }
    public LayerMask StructureMask => structureMask;
    public float RepathInterval => repathInterval;

    // Gli stati vengono creati una volta sola e riusati (niente allocazioni durante il gioco)
    public SeekState Seek { get; private set; }
    public ChaseState Chase { get; private set; }
    public AttackState Attack { get; private set; }
    public DeadState Dead { get; private set; }

    public string StateName => current != null ? current.Name : "-";

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Health = GetComponent<Health>();
        Seek = new SeekState(this);
        Chase = new ChaseState(this);
        Attack = new AttackState(this);
        Dead = new DeadState(this);
    }

    private void OnEnable() => Health.Died += OnDied;
    private void OnDisable() => Health.Died -= OnDied;

    /// <summary>Chiamato da Enemy dopo lo spawn dal pool.</summary>
    public void Setup(EnemyData data, Health crystal, Health player)
    {
        Data = data;
        Crystal = crystal;
        Player = player;

        // Un agente riciclato va riposizionato con Warp, non spostando il transform
        Agent.Warp(transform.position);
        Agent.speed = data.MoveSpeed;
        Agent.stoppingDistance = data.AttackRange * 0.8f;
        Agent.isStopped = false;

        current = null;
        ChangeState(Seek);
    }

    private void Update()
    {
        current?.Tick();
    }

    public void ChangeState(EnemyState next)
    {
        if (next == current) return;
        current?.Exit();
        current = next;
        current.Enter();
    }

    private void OnDied() => ChangeState(Dead);

    /// <summary>Distanza dalla superficie del bersaglio (il cristallo è grande: conta il bordo, non il centro).</summary>
    public float DistanceTo(Health target)
    {
        Vector3 point = target.transform.position;
        if (target.TryGetComponent(out Collider col))
        {
            point = col.ClosestPoint(transform.position);
        }
        point.y = transform.position.y;
        return Vector3.Distance(transform.position, point);
    }

    public bool IsAlive(Health target) => target != null && !target.IsDead && target.gameObject.activeInHierarchy;

    private void OnDrawGizmos()
    {
        if (current == null) return;
        Gizmos.color = current == Seek ? Color.yellow : current == Chase ? new Color(1f, 0.5f, 0f) : current == Attack ? Color.red : Color.gray;
        Gizmos.DrawSphere(transform.position + Vector3.up * 2.6f * transform.localScale.y, 0.2f);
    }
}
