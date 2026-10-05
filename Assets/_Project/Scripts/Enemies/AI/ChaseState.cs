using UnityEngine;

/// <summary>
/// CHASE: insegue il player. Se il player si allontana troppo torna a SEEK, se è a portata attacca.
/// </summary>
public class ChaseState : EnemyState
{
    private const float LeashMultiplier = 1.6f;
    private float repathTimer;

    public ChaseState(EnemyAI ai) : base(ai) { }

    public override string Name => "CHASE";

    public override void Enter()
    {
        repathTimer = 0f;
        ai.Agent.isStopped = false;
    }

    public override void Tick()
    {
        if (!ai.IsAlive(ai.Player))
        {
            ai.ChangeState(ai.Seek);
            return;
        }

        float distance = ai.DistanceTo(ai.Player);

        // Chi ha come priorità il player non smette mai di inseguirlo
        if (ai.Data.Priority != EnemyData.TargetPriority.Player && distance > ai.Data.AggroRange * LeashMultiplier)
        {
            ai.ChangeState(ai.Seek);
            return;
        }

        if (distance <= ai.Data.AttackRange)
        {
            ai.Attack.SetTarget(ai.Player, ai.Chase);
            ai.ChangeState(ai.Attack);
            return;
        }

        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            repathTimer = ai.RepathInterval;
            ai.Agent.SetDestination(ai.Player.transform.position);
        }
    }
}
