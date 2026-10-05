using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Mette in pausa il gioco con l'action Pause (Esc / Start) e mostra il pannello di pausa.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;

    private GameControls controls;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        controls = new GameControls();
    }

    private void OnEnable()
    {
        // Iscrizione all'evento dell'action e al click del bottone...
        controls.Player.Pause.performed += OnPausePerformed;
        controls.Player.Pause.Enable();
        resumeButton.onClick.AddListener(Resume);
    }

    private void OnDisable()
    {
        // ...e disiscrizione simmetrica, per non lasciare riferimenti appesi.
        controls.Player.Pause.performed -= OnPausePerformed;
        controls.Player.Pause.Disable();
        resumeButton.onClick.RemoveListener(Resume);
    }

    private void OnDestroy() => controls.Dispose();

    private void Start() => SetPaused(false);

    private void OnPausePerformed(InputAction.CallbackContext context) => SetPaused(!IsPaused);

    public void Resume() => SetPaused(false);

    private void SetPaused(bool paused)
    {
        IsPaused = paused;
        pausePanel.SetActive(paused);

        // timeScale = 0 ferma tutto ciò che usa Time.deltaTime (movimento, fisica, timer).
        Time.timeScale = paused ? 0f : 1f;
    }
}
