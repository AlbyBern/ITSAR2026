using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Tooltip("Velocità di rotazione in gradi al secondo")]
    [SerializeField] private float rotationSpeed = 45f;

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}