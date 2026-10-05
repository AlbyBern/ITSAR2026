using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Singleton persistente che gestisce TUTTI i cambi di scena, con dissolvenza e barra di caricamento.
/// Un'istanza è presente in ogni scena (così ogni scena si può avviare da sola nell'Editor):
/// la prima sopravvive con DontDestroyOnLoad, le copie successive si distruggono in Awake.
/// </summary>
public class SceneChanger : MonoBehaviour
{
    [SerializeField] private CanvasGroup fade;
    [SerializeField] private Image progressFill;
    [SerializeField] private float fadeDuration = 0.3f;

    public static SceneChanger Instance { get; private set; }

    public bool IsLoading { get; private set; }

    /// <summary>Avanzamento del caricamento, da 0 a 1.</summary>
    public event Action<float> ProgressChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Esiste già: questa è una copia arrivata con la nuova scena
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetFade(0f);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    /// <summary>Carica una scena in modo asincrono. Le richieste durante un caricamento vengono ignorate.</summary>
    public void LoadScene(string sceneName)
    {
        if (IsLoading) return;
        StartCoroutine(LoadRoutine(sceneName));
    }

    private IEnumerator LoadRoutine(string sceneName)
    {
        IsLoading = true;
        yield return Fade(0f, 1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            // Il caricamento vero e proprio arriva a 0.9, l'attivazione fa il resto
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (progressFill != null) progressFill.fillAmount = progress;
            ProgressChanged?.Invoke(progress);
            yield return null;
        }

        // Una scena caricata da un menu di pausa non deve restare ferma
        if (!PauseService.IsPaused) Time.timeScale = 1f;

        yield return Fade(1f, 0f);
        IsLoading = false;
    }

    private IEnumerator Fade(float from, float to)
    {
        // Tempo non scalato: la dissolvenza funziona anche con il gioco in pausa
        for (float t = 0f; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            SetFade(Mathf.Lerp(from, to, t / fadeDuration));
            yield return null;
        }
        SetFade(to);
    }

    private void SetFade(float alpha)
    {
        if (fade == null) return;
        fade.alpha = alpha;
        fade.blocksRaycasts = alpha > 0.01f;
    }
}
