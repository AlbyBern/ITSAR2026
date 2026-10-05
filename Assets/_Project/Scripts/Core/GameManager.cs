using System.Collections;
using UnityEngine;

/// <summary>
/// Regole della partita nella scena Arena: conta tempo e nemici uccisi e, quando muore
/// il player o il cristallo, salva i risultati e passa alla scena GameOver.
/// Non è persistente: vive e muore con la scena Arena.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private Health player;
    [SerializeField] private Health crystal;
    [SerializeField] private PlayerExperience experience;
    [SerializeField] private WaveSpawner waves;
    [SerializeField] private string gameOverScene = "GameOver";
    [SerializeField, Tooltip("Secondi tra la sconfitta e il cambio di scena")] private float delay = 1.5f;

    private float elapsed;
    private int kills;
    private bool over;

    private void OnEnable()
    {
        player.Died += OnPlayerDied;
        crystal.Died += OnCrystalDied;
        Enemy.Killed += OnEnemyKilled;
    }

    private void OnDisable()
    {
        player.Died -= OnPlayerDied;
        crystal.Died -= OnCrystalDied;
        // Evento statico: dimenticare la disiscrizione lascerebbe un riferimento a un oggetto distrutto
        Enemy.Killed -= OnEnemyKilled;
    }

    private void Update()
    {
        if (!over) elapsed += Time.deltaTime;
    }

    private void OnEnemyKilled(Enemy enemy) => kills++;
    private void OnPlayerDied() => EndRun("Sei stato sconfitto");
    private void OnCrystalDied() => EndRun("Il cristallo è stato distrutto");

    private void EndRun(string reason)
    {
        if (over) return;
        over = true;

        RunResults.Reason = reason;
        RunResults.TimeSurvived = elapsed;
        RunResults.Kills = kills;
        RunResults.Level = experience.Level;
        RunResults.Wave = waves.WaveNumber;

        // Il player sconfitto non si muove e non spara più
        player.gameObject.SetActive(false);
        StartCoroutine(LoadGameOver());
    }

    private IEnumerator LoadGameOver()
    {
        yield return new WaitForSeconds(delay);
        SceneChanger.Instance.LoadScene(gameOverScene);
    }
}
