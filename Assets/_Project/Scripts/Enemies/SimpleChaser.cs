using UnityEngine;

/// <summary>
/// [Fornito dal docente] Movimento provvisorio dei nemici: va dritto verso il bersaglio.
/// Usa un Rigidbody cinematico mosso in FixedUpdate. Attraversa gli ostacoli:
/// nella lezione 08 verrà sostituito da NavMesh e macchina a stati.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SimpleChaser : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 2.5f;
    [SerializeField, Tooltip("Distanza a cui si ferma dal bersaglio")] private float stopDistance = 1.4f;

    private Rigidbody body;

    public void SetTarget(Transform newTarget) => target = newTarget;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 toTarget = target.position - body.position;
        toTarget.y = 0f;
        if (toTarget.magnitude <= stopDistance) return;

        Vector3 step = toTarget.normalized * speed * Time.fixedDeltaTime;
        body.MovePosition(body.position + step);
        body.MoveRotation(Quaternion.LookRotation(toTarget));
    }
}
