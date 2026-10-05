using UnityEngine;
using UnityEngine.UI;

/// <summary>Schermata di fine partita: mostra i risultati e permette di riprovare o tornare al menu.</summary>
public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Text reasonLabel;
    [SerializeField] private Text statsLabel;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private string gameScene = "Arena";
    [SerializeField] private string menuScene = "MainMenu";

    private void OnEnable()
    {
        retryButton.onClick.AddListener(Retry);
        menuButton.onClick.AddListener(BackToMenu);
    }

    private void OnDisable()
    {
        retryButton.onClick.RemoveListener(Retry);
        menuButton.onClick.RemoveListener(BackToMenu);
    }

    private void Start()
    {
        int minutes = (int)(RunResults.TimeSurvived / 60f);
        int seconds = (int)(RunResults.TimeSurvived % 60f);

        reasonLabel.text = RunResults.Reason;
        statsLabel.text =
            $"Tempo: {minutes}:{seconds:00}\n" +
            $"Nemici sconfitti: {RunResults.Kills}\n" +
            $"Livello: {RunResults.Level}\n" +
            $"Ondata: {RunResults.Wave}";
    }

    private void Retry() => SceneChanger.Instance.LoadScene(gameScene);
    private void BackToMenu() => SceneChanger.Instance.LoadScene(menuScene);
}
