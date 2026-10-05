using UnityEngine;
using UnityEngine.UI;

/// <summary>Menu principale: avvia la partita o chiude il gioco.</summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private string gameScene = "Arena";

    private void OnEnable()
    {
        playButton.onClick.AddListener(Play);
        quitButton.onClick.AddListener(Quit);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(Play);
        quitButton.onClick.RemoveListener(Quit);
    }

    private void Play() => SceneChanger.Instance.LoadScene(gameScene);

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
