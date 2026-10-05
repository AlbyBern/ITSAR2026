using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Al passaggio di livello mette in pausa il gioco e propone potenziamenti casuali (UpgradeData).
/// </summary>
public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private PlayerExperience experience;
    [SerializeField] private GameObject panel;
    [SerializeField, Tooltip("Ogni bottone ha due Text figli: Title e Description")] private Button[] optionButtons;
    [SerializeField] private List<UpgradeData> upgrades = new List<UpgradeData>();

    private readonly List<UpgradeData> offered = new List<UpgradeData>();
    private int pendingChoices;

    private void OnEnable()
    {
        experience.LeveledUp += OnLeveledUp;
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // copia locale: la lambda deve catturare il valore di questo giro
            optionButtons[i].onClick.AddListener(() => Choose(index));
        }
    }

    private void OnDisable()
    {
        experience.LeveledUp -= OnLeveledUp;
        foreach (Button button in optionButtons) button.onClick.RemoveAllListeners();
        PauseService.Release(this);
    }

    private void Start() => panel.SetActive(false);

    private void OnLeveledUp(int level)
    {
        pendingChoices++;
        if (!panel.activeSelf) Show();
    }

    private void Show()
    {
        GameObject player = experience.gameObject;

        // Candidati disponibili, mescolati (Fisher-Yates)
        var candidates = new List<UpgradeData>();
        foreach (UpgradeData u in upgrades)
        {
            if (u.IsAvailable(player)) candidates.Add(u);
        }
        for (int i = candidates.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (candidates[i], candidates[j]) = (candidates[j], candidates[i]);
        }

        offered.Clear();
        for (int i = 0; i < optionButtons.Length; i++)
        {
            bool hasOption = i < candidates.Count;
            optionButtons[i].gameObject.SetActive(hasOption);
            if (!hasOption) continue;

            offered.Add(candidates[i]);
            optionButtons[i].transform.Find("Title").GetComponent<Text>().text = candidates[i].Title;
            optionButtons[i].transform.Find("Description").GetComponent<Text>().text = candidates[i].Description;
        }

        if (offered.Count == 0)
        {
            pendingChoices = 0;
            return;
        }

        panel.SetActive(true);
        PauseService.Request(this);
    }

    private void Choose(int index)
    {
        offered[index].Apply(experience.gameObject);
        pendingChoices--;

        panel.SetActive(false);
        PauseService.Release(this);

        // Più livelli presi insieme: una scelta per ciascuno
        if (pendingChoices > 0) Show();
    }
}
