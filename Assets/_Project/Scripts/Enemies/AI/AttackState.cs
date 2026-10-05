using UnityEngine;

/// <summary>
/// ATTACK: fermo, colpisce il bersaglio a intervalli regolari.
/// Se il bersaglio muore o si allontana, torna allo stato da cui era arrivato.
/// </summary>
public class AttackState : EnemyState
{
    private const float BreakRangeMultiplier = 1.3f;

    private Health target;
    private EnemyState returnState;
    private float attackTimer;

    public AttackState(EnemyAI ai) : base(ai) { }

    public override string Name => "ATTACK";

    /// <summary>Da chiamare prima di entrare nello stato.</summary>
    public void SetTarget(Health newTarget, EnemyState stateToReturnTo)
    {
        target = newTarget;
        returnState = stateToReturnTo;
    }

    public override void Enter()
    {
        ai.Agent.isStopped = true;
        ai.Agent.ResetPath();
        attackTimer = 0.2f; // piccolo ritardo prima del primo colpo
    }

    public override void Tick()
    {
        if (!ai.IsAlive(target) || ai.DistanceTo(target) > ai.Data.AttackRange * BreakRangeMultiplier)
        {
            ai.ChangeState(returnState);
            return;
        }

        // Guarda il bersaglio
        Vector3 look = target.transform.position - ai.transform.position;
        look.y = 0f;
        if (look.sqrMagnitude > 0.01f)
        {
            ai.transform.rotation = Quaternion.RotateTowards(ai.transform.rotation, Quaternion.LookRotation(look), 540f * Time.deltaTime);
        }

        attackTimer -= Time.deltaTime;
        if (attackTimer > 0f) return;

        attackTimer = ai.Data.AttackInterval;
        target.TakeDamage(ai.Data.ContactDamagePerSecond * ai.Data.AttackInterval);
    }

    // Exit è importante: senza, l'agente resterebbe fermo anche negli stati successivi
    public override void Exit()
    {
        ai.Agent.isStopped = false;
    }
}
