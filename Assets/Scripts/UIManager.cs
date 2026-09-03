using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Panels (Attach Canvas Groups)")]
    [SerializeField] private CanvasGroup screenCharacterSelect;
    [SerializeField] private CanvasGroup screenEmotionLab;
    [SerializeField] private CanvasGroup screenSettings;
    [SerializeField] private float fadeDuration = 0.25f;

    [Header("Dynamic Info Elements (Screen 2)")]
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private TMP_Text moodStatusText;

    private CanvasGroup activeScreen;

    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
            return; 
        }
        Instance = this;
    }

    void Start()
    {
    
        ShowImmediate(screenCharacterSelect);
    }

    
    public void GoToCharacterSelect() => SwitchScreen(screenCharacterSelect);
    public void GoToEmotionLab() => SwitchScreen(screenEmotionLab);
    public void GoToSettings() => SwitchScreen(screenSettings);

    private void SwitchScreen(CanvasGroup targetScreen)
    {
        if (targetScreen == null || activeScreen == targetScreen) return;
        StopAllCoroutines();
        StartCoroutine(TransitionRoutine(targetScreen));
    }

    private IEnumerator TransitionRoutine(CanvasGroup target)
    {
        
        if (activeScreen != null)
        {
            yield return StartCoroutine(FadeGroup(activeScreen, 1f, 0f));
            activeScreen.gameObject.SetActive(false);
        }

        // Fade in target panel
        target.gameObject.SetActive(true);
        yield return StartCoroutine(FadeGroup(target, 0f, 1f));
        activeScreen = target;
    }

    private IEnumerator FadeGroup(CanvasGroup cg, float start, float end)
    {
        float elapsed = 0f;
        cg.alpha = start;
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

    private void ShowImmediate(CanvasGroup target)
    {
        
        SetScreenState(screenCharacterSelect, false);
        SetScreenState(screenEmotionLab, false);
        SetScreenState(screenSettings, false);

       
        if (target != null)
        {
            SetScreenState(target, true);
            activeScreen = target;
        }
    }

    private void SetScreenState(CanvasGroup cg, bool visible)
    {
        if (cg == null) return;
        cg.gameObject.SetActive(visible);
        cg.alpha = visible ? 1f : 0f;
        cg.interactable = visible;
        cg.blocksRaycasts = visible;
    }

  
    public void UpdateCharacterDisplay(string charName)
    {
        if (characterNameText != null)
            characterNameText.text = $"Selected: {charName}";
    }

    public void UpdateMoodDisplay(string mood)
    {
        if (moodStatusText != null)
            moodStatusText.text = $"Current Mood: {mood}";
    }
}