using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Punto unico per mettere in pausa il gioco. Più sistemi possono chiedere la pausa
/// (menu di pausa, scelta del potenziamento): il gioco riparte solo quando tutti l'hanno rilasciata.
/// Così due sistemi non si "rubano" Time.timeScale a vicenda.
/// </summary>
public static class PauseService
{
    private static readonly HashSet<object> requests = new HashSet<object>();

    public static bool IsPaused => requests.Count > 0;

    public static void Request(object requester)
    {
        requests.Add(requester);
        Apply();
    }

    public static void Release(object requester)
    {
        requests.Remove(requester);
        Apply();
    }

    private static void Apply()
    {
        Time.timeScale = IsPaused ? 0f : 1f;
    }

    // Le classi statiche sopravvivono tra una partita e l'altra nell'Editor: azzeriamo all'avvio.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay()
    {
        requests.Clear();
        Time.timeScale = 1f;
    }
}
