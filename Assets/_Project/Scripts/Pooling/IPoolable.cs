/// <summary>
/// Implementato dai componenti che devono reimpostare il proprio stato
/// quando l'oggetto esce dal pool (OnSpawned) o ci rientra (OnDespawned).
/// </summary>
public interface IPoolable
{
    void OnSpawned();
    void OnDespawned();
}
