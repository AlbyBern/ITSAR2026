using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Mette in pausa il gioco con l'action Pause (Esc / Start) e mostra il pannello di pausa.
/// La pausa passa da PauseService, condiviso con la schermata dei potenziamenti.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private string menuScene = "MainMenu";

    private GameControls controls;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        controls = new GameControls();
    }

    private void OnEnable()
    {
        controls.Player.Pause.performed += OnPausePerformed;
        controls.Player.Pause.Enable();
        resumeButton.onClick.AddListener(Resume);
        menuButton.onClick.AddListener(BackToMenu);
    }

    private void OnDisable()
    {
        controls.Player.Pause.performed -= OnPausePerformed;
        controls.Player.Pause.Disable();
        resumeButton.onClick.RemoveListener(Resume);
        menuButton.onClick.RemoveListener(BackToMenu);
        PauseService.Release(this);
    }

    private void OnDestroy() => controls.Dispose();

    private void Start() => SetOpen(false);

    private void OnPausePerformed(InputAction.CallbackContext context) => SetOpen(!IsOpen);

    public void Resume() => SetOpen(false);

    private void BackToMenu() => SceneChanger.Instance.LoadScene(menuScene);

    private void SetOpen(bool open)
    {
        IsOpen = open;
        pausePanel.SetActive(open);

        if (open) PauseService.Request(this);
        else PauseService.Release(this);
    }
}
