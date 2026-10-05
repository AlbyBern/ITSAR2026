using UnityEngine;
using UnityEngine.UI;

/// <summary>Barra dell'esperienza e indicatore del livello, aggiornati tramite eventi.</summary>
public class ExperienceBarUI : MonoBehaviour
{
    [SerializeField] private PlayerExperience experience;
    [SerializeField] private Image fill;
    [SerializeField] private Text levelLabel;

    private void OnEnable()
    {
        experience.ExperienceChanged += OnExperienceChanged;
        experience.LeveledUp += OnLeveledUp;
    }

    private void OnDisable()
    {
        experience.ExperienceChanged -= OnExperienceChanged;
        experience.LeveledUp -= OnLeveledUp;
    }

    private void Start()
    {
        OnExperienceChanged(experience.Experience, experience.Required);
        OnLeveledUp(experience.Level);
    }

    private void OnExperienceChanged(int current, int required)
    {
        fill.fillAmount = required > 0 ? (float)current / required : 0f;
    }

    private void OnLeveledUp(int level)
    {
        levelLabel.text = "Lv " + level;
    }
}
