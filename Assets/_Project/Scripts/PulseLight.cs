using UnityEngine;

public class PulseLight : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Se vuoto, cerca il Renderer in questo oggetto o nei figli (es. Mesh)")]
    [SerializeField] private Renderer targetRenderer;

    [Header("Emission")]
    [ColorUsage(false, true)]
    [SerializeField] private Color emissionColor = Color.cyan;

    [Min(0f)] [SerializeField] private float minIntensity = 0.5f;
    [Min(0f)] [SerializeField] private float maxIntensity = 3f;

    [Header("Pulse")]
    [Tooltip("Pulsazioni complete al secondo")]
    [Min(0f)] [SerializeField] private float pulsesPerSecond = 0.5f;

    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    private Material materialInstance;

    private void Awake()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<Renderer>();

        // .material crea un'istanza: non modifichi l'asset su disco
        materialInstance = targetRenderer.material;
        materialInstance.EnableKeyword("_EMISSION");
    }

    private void Update()
    {
        // sin va da -1 a 1 -> lo rimappo su 0..1
        float t = (Mathf.Sin(Time.time * pulsesPerSecond * Mathf.PI * 2f) + 1f) * 0.5f;
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

        materialInstance.SetColor(EmissionColorId, emissionColor * intensity);
    }

    private void OnDestroy()
    {
        if (materialInstance != null)
            Destroy(materialInstance);
    }
}