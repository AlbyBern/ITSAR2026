using UnityEngine;

/// <summary>
/// DEAD: il nemico si ferma, si rimpicciolisce e torna nel pool. Da qui non si esce.
/// </summary>
public class DeadState : EnemyState
{
    private const float ShrinkTime = 0.25f;

    private float timer;
    private Vector3 startScale;

    public DeadState(EnemyAI ai) : base(ai) { }

    public override string Name => "DEAD";

    public override void Enter()
    {
        ai.Agent.isStopped = true;
        ai.Agent.ResetPath();
        timer = 0f;
        startScale = ai.transform.localScale;
    }

    public override void Tick()
    {
        timer += Time.deltaTime;
        ai.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, timer / ShrinkTime);

        if (timer >= ShrinkTime)
        {
            Pool.Despawn(ai.gameObject);
        }
    }
}
