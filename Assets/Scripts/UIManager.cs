using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Screens (Canvas Groups)")]
    [SerializeField] private CanvasGroup screenCharacterSelect;
    [SerializeField] private CanvasGroup screenEmotionLab;
    [SerializeField] private CanvasGroup screenSettings;
    [SerializeField] private float fadeDuration = 0.3f;

    [Header("Dynamic UI Elements (Screen 2 Reaction)")]
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private TMP_Text characterBioText;
    [SerializeField] private TMP_Text moodStatusText;
    [SerializeField] private Image moodAccentBorder;
    [SerializeField] private Slider intensitySlider;

    private CanvasGroup activeScreen;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        
        ShowScreenImmediate(screenCharacterSelect);
    }

    public void GoToCharacterSelect() => SwitchScreen(screenCharacterSelect);
    public void GoToEmotionLab() => SwitchScreen(screenEmotionLab);
    public void GoToSettings() => SwitchScreen(screenSettings);

    private void SwitchScreen(CanvasGroup targetScreen)
    {
        if (activeScreen == targetScreen) return;
        StopAllCoroutines();
        StartCoroutine(TransitionRoutine(targetScreen));
    }

    private IEnumerator TransitionRoutine(CanvasGroup target)
    {
        if (activeScreen != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(activeScreen, 1f, 0f));
            activeScreen.gameObject.SetActive(false);
        }

        target.gameObject.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(target, 0f, 1f));
        activeScreen = target;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            yield return null;
        }
        cg.alpha = end;
        cg.interactable = (end == 1f);
        cg.blocksRaycasts = (end == 1f);
    }

    private void ShowScreenImmediate(CanvasGroup target)
    {
        screenCharacterSelect.gameObject.SetActive(false);
        screenEmotionLab.gameObject.SetActive(false);
        screenSettings.gameObject.SetActive(false);

        target.gameObject.SetActive(true);
        target.alpha = 1f;
        target.interactable = true;
        target.blocksRaycasts = true;
        activeScreen = target;
    }

    
    public void UpdateCharacterDisplay(string charName, string bio)
    {
        if (characterNameText != null) characterNameText.text = charName;
        if (characterBioText != null) characterBioText.text = bio;
    }

    public void UpdateMoodDisplay(string moodName, Color moodColor)
    {
        if (moodStatusText != null) moodStatusText.text = $"Mood: {moodName.ToUpper()}";
        if (moodAccentBorder != null) moodAccentBorder.color = moodColor;
    }
}