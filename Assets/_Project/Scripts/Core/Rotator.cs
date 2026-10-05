using UnityEngine;

/// <summary>
/// Ruota il GameObject attorno a un asse a velocità costante, indipendentemente dal framerate.
/// </summary>
public class Rotator : MonoBehaviour
{
    [SerializeField, Tooltip("Velocità di rotazione in gradi al secondo")]
    private float degreesPerSecond = 45f;

    [SerializeField, Tooltip("Asse di rotazione (in spazio locale)")]
    private Vector3 axis = Vector3.up;

    private void Update()
    {
        // Time.deltaTime = secondi trascorsi dall'ultimo frame:
        // moltiplicando, la rotazione dipende dal tempo e non dal numero di frame.
        transform.Rotate(axis.normalized, degreesPerSecond * Time.deltaTime, Space.Self);
    }
}
