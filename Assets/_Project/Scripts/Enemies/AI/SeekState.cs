using UnityEngine;

/// <summary>
/// SEEK: raggiunge il bersaglio principale in base alla priorità del tipo di nemico
/// (cristallo, oppure la torre più vicina). Se vede il player vicino, passa a CHASE.
/// </summary>
public class SeekState : EnemyState
{
    private readonly Collider[] buffer = new Collider[16];
    private Health target;
    private float repathTimer;

    public SeekState(EnemyAI ai) : base(ai) { }

    public override string Name => "SEEK";

    public override void Enter()
    {
        target = ChooseTarget();
        repathTimer = 0f;
        ai.Agent.isStopped = false;
    }

    public override void Tick()
    {
        // Priorità: player vicino (tranne chi punta le torri)
        if (ai.Data.Priority != EnemyData.TargetPriority.Towers && ai.IsAlive(ai.Player)
            && ai.DistanceTo(ai.Player) <= ai.Data.AggroRange)
        {
            ai.ChangeState(ai.Chase);
            return;
        }

        if (!ai.IsAlive(target))
        {
            target = ChooseTarget();
            if (target == null) return;
        }

        if (ai.DistanceTo(target) <= ai.Data.AttackRange)
        {
            ai.Attack.SetTarget(target, ai.Seek);
            ai.ChangeState(ai.Attack);
            return;
        }

        // Il percorso non si ricalcola a ogni frame: costa
        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            repathTimer = ai.RepathInterval;
            ai.Agent.SetDestination(target.transform.position);
        }
    }

    private Health ChooseTarget()
    {
        if (ai.Data.Priority == EnemyData.TargetPriority.Player && ai.IsAlive(ai.Player)) return ai.Player;

        if (ai.Data.Priority == EnemyData.TargetPriority.Towers)
        {
            Health tower = NearestStructure();
            if (tower != null) return tower;
        }
        return ai.IsAlive(ai.Crystal) ? ai.Crystal : null;
    }

    private Health NearestStructure()
    {
        int count = Physics.OverlapSphereNonAlloc(ai.transform.position, 30f, buffer, ai.StructureMask, QueryTriggerInteraction.Ignore);
        Health best = null;
        float bestDistance = float.MaxValue;
        for (int i = 0; i < count; i++)
        {
            Health h = buffer[i].GetComponentInParent<Health>();
            if (h == null || h == ai.Crystal || !ai.IsAlive(h)) continue;
            float d = (h.transform.position - ai.transform.position).sqrMagnitude;
            if (d < bestDistance)
            {
                best = h;
                bestDistance = d;
            }
        }
        return best;
    }
}
