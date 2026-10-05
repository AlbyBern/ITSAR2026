using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// [Fornito dal docente] Mostra a schermo FPS, tempo del frame e memoria allocata per frame (GC Alloc).
/// Usa ProfilerRecorder, che legge gli stessi contatori del Profiler anche fuori dall'Editor.
/// Il testo si aggiorna due volte al secondo: anche creare stringhe alloca memoria!
/// </summary>
public class PerformanceOverlay : MonoBehaviour
{
    [SerializeField] private Text label;
    [SerializeField] private float refreshInterval = 0.5f;

    private ProfilerRecorder gcAllocRecorder;
    private float timer;
    private int frames;
    private long gcBytes;
    private long maxGcBytes;

    private void OnEnable()
    {
        gcAllocRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocated In Frame");
    }

    private void OnDisable()
    {
        gcAllocRecorder.Dispose();
    }

    private void Update()
    {
        frames++;
        timer += Time.unscaledDeltaTime;
        long bytes = gcAllocRecorder.Valid ? gcAllocRecorder.LastValue : 0;
        gcBytes += bytes;
        if (bytes > maxGcBytes) maxGcBytes = bytes;

        if (timer < refreshInterval) return;

        float fps = frames / timer;
        float ms = timer * 1000f / frames;
        float kbPerFrame = gcBytes / 1024f / frames;
        int enemies = CountEnemies();
        label.text = $"{fps:0} FPS  {ms:0.0} ms\nGC Alloc: {kbPerFrame:0.0} KB/frame (picco {maxGcBytes / 1024f:0.0} KB)\nNemici attivi: {enemies}";

        timer = 0f;
        frames = 0;
        gcBytes = 0;
        maxGcBytes = 0;
    }

    // Una ricerca ogni mezzo secondo va bene per uno strumento di debug, non per il gameplay
    private static int CountEnemies()
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int count = 0;
        foreach (Health h in Object.FindObjectsByType<Health>())
        {
            if (h.gameObject.layer == enemyLayer) count++;
        }
        return count;
    }
}
