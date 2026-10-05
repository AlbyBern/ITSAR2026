using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Costruisce una torre davanti al giocatore spendendo gemme (action PlaceTower: E / Y).
/// Prima di costruire verifica con query fisiche che ci sia terreno e che lo spazio sia libero.
/// </summary>
[RequireComponent(typeof(GemCollector))]
public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private int cost = 10;
    [SerializeField, Tooltip("Distanza davanti al player")] private float placeDistance = 2.5f;
    [SerializeField, Tooltip("Le torri si allineano a una griglia di questo passo")] private float gridSize = 2f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField, Tooltip("Layer che occupano lo spazio (ostacoli, strutture, player)")] private LayerMask blockingMask;
    [SerializeField] private float clearRadius = 0.9f;

    private GemCollector gems;
    private GameControls controls;

    private void Awake()
    {
        gems = GetComponent<GemCollector>();
        controls = new GameControls();
    }

    private void OnEnable()
    {
        controls.Player.PlaceTower.performed += OnPlaceTower;
        controls.Player.PlaceTower.Enable();
    }

    private void OnDisable()
    {
        controls.Player.PlaceTower.performed -= OnPlaceTower;
        controls.Player.PlaceTower.Disable();
    }

    private void OnDestroy() => controls.Dispose();

    private void OnPlaceTower(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f) return; // niente costruzioni in pausa

        Vector3 position = SnapToGrid(transform.position + transform.forward * placeDistance);

        if (!CanPlace(position))
        {
            Debug.Log("Torre: spazio non valido");
            return;
        }
        if (!gems.TrySpend(cost))
        {
            Debug.Log($"Torre: servono {cost} gemme");
            return;
        }

        Instantiate(towerPrefab, position, Quaternion.identity);
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        position.x = Mathf.Round(position.x / gridSize) * gridSize;
        position.z = Mathf.Round(position.z / gridSize) * gridSize;
        position.y = 0f;
        return position;
    }

    private bool CanPlace(Vector3 position)
    {
        // Raycast verso il basso: sotto deve esserci terreno
        bool hasGround = Physics.Raycast(position + Vector3.up * 5f, Vector3.down, 10f, groundMask, QueryTriggerInteraction.Ignore);

        // Sfera di controllo: lo spazio non deve essere occupato
        bool occupied = Physics.CheckSphere(position + Vector3.up * 1f, clearRadius, blockingMask, QueryTriggerInteraction.Ignore);

        return hasGround && !occupied;
    }
}
