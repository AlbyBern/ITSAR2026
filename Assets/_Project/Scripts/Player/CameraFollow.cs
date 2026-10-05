using UnityEngine;

/// <summary>
/// Camera dall'alto che segue il bersaglio con un movimento morbido.
/// La rotazione della camera resta quella impostata nella scena.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 16f, -11f);
    [SerializeField, Tooltip("Tempo di inseguimento: più è alto, più la camera è morbida")]
    private float smoothTime = 0.15f;

    private Vector3 velocity;

    // LateUpdate: viene eseguito dopo tutti gli Update, quando il player si è già mosso.
    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }
}
