using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Characters")]
    [SerializeField] private CharacterMoodController[] characters;
    private int activeIndex = 0;

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
        SelectCharacter(0);
    }

    void Update()
    {
        HandleDirectKeyboardInput();
    }

    private void HandleDirectKeyboardInput()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        
        if (keyboard.digit1Key.wasPressedThisFrame) SelectCharacter(0);
        if (keyboard.digit2Key.wasPressedThisFrame) SelectCharacter(1);
        if (keyboard.digit3Key.wasPressedThisFrame) SelectCharacter(2);

      
        if (keyboard.jKey.wasPressedThisFrame) TriggerJoy();
        if (keyboard.aKey.wasPressedThisFrame) TriggerAnger();
        if (keyboard.sKey.wasPressedThisFrame) TriggerSadness();

      
        if (keyboard.upArrowKey.wasPressedThisFrame) SetIntensity(1.5f);
        if (keyboard.downArrowKey.wasPressedThisFrame) SetIntensity(0.75f);
    }

    public void SelectCharacter(int index)
    {
        if (characters == null || characters.Length == 0) return;
        index = Mathf.Clamp(index, 0, characters.Length - 1);

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] != null)
                characters[i].gameObject.SetActive(i == index);
        }

        activeIndex = index;
        Debug.Log($"Active Character switched to: {characters[activeIndex].characterName}");

        if (UIManager.Instance != null)
        {
            var activeChar = characters[activeIndex];
            UIManager.Instance.UpdateCharacterDisplay(activeChar.characterName);
            UIManager.Instance.UpdateMoodDisplay("Idle");
        }
    }

    public void TriggerJoy()
    {
        characters[activeIndex].PlayJoy();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayCheerSFX();
        if (UIManager.Instance != null) UIManager.Instance.UpdateMoodDisplay("Joy");
        Debug.Log("Triggered: Joy");
    }

    public void TriggerAnger()
    {
        characters[activeIndex].PlayAnger();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayAngerSFX();
        if (UIManager.Instance != null) UIManager.Instance.UpdateMoodDisplay("Anger");
        Debug.Log("Triggered: Anger");
    }

    public void TriggerSadness()
    {
        characters[activeIndex].PlaySadness();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayCrySFX();
        if (UIManager.Instance != null) UIManager.Instance.UpdateMoodDisplay("Sadness");
        Debug.Log("Triggered: Sadness");
    }

    public void SetIntensity(float value)
    {
        characters[activeIndex].SetIntensity(value);
        Debug.Log($"Mood Intensity set to: {value}");
    }
}