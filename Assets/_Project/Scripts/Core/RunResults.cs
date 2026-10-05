/// <summary>
/// Risultati dell'ultima partita, letti dalla scena GameOver.
/// Classe statica: un modo semplice per passare pochi dati da una scena all'altra.
/// </summary>
public static class RunResults
{
    public static string Reason { get; set; } = "";
    public static float TimeSurvived { get; set; }
    public static int Kills { get; set; }
    public static int Level { get; set; }
    public static int Wave { get; set; }
}
