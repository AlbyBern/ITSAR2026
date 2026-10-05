/// <summary>
/// Uno stato della macchina a stati del nemico.
/// Enter all'ingresso, Tick a ogni frame, Exit all'uscita: ogni stato contiene solo la sua logica.
/// </summary>
public abstract class EnemyState
{
    protected readonly EnemyAI ai;

    protected EnemyState(EnemyAI ai)
    {
        this.ai = ai;
    }

    public abstract string Name { get; }

    public virtual void Enter() { }
    public abstract void Tick();
    public virtual void Exit() { }
}
