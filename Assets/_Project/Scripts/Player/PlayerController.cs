using UnityEngine;

/// <summary>
/// Movimento del giocatore con CharacterController e nuovo Input System.
/// Il movimento è relativo alla camera: "su" significa sempre "verso l'alto dello schermo".
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float sprintMultiplier = 1.6f;
    [SerializeField, Tooltip("Gradi al secondo")] private float rotationSpeed = 720f;
    [SerializeField] private float gravity = -20f;

    [Header("Riferimenti")]
    [SerializeField, Tooltip("Se vuoto usa la Main Camera")] private Transform cameraTransform;

    private CharacterController controller;
    private GameControls controls;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new GameControls();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Le action vanno abilitate e disabilitate insieme al componente.
    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();
    private void OnDestroy() => controls.Dispose();

    private void Update()
    {
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();
        Vector3 direction = CameraRelative(input);

        float speed = moveSpeed;
        if (controls.Player.Sprint.IsPressed())
        {
            speed *= sprintMultiplier;
        }

        // Gravità applicata a mano: il CharacterController non usa la fisica.
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = direction * speed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>Converte l'input 2D in una direzione sul piano XZ, orientata come la camera.</summary>
    private Vector3 CameraRelative(Vector2 input)
    {
        if (cameraTransform == null)
        {
            return new Vector3(input.x, 0f, input.y);
        }

        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0f;
        right.Normalize();

        return Vector3.ClampMagnitude(forward * input.y + right * input.x, 1f);
    }
}
