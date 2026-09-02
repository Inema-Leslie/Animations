using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Characters")]
    [SerializeField] private CharacterMoodController[] characters;
    private int activeIndex = 0;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
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
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectCharacter(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectCharacter(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectCharacter(2);

        // Emotion trigger keys
        if (Input.GetKeyDown(KeyCode.J)) TriggerJoy();
        if (Input.GetKeyDown(KeyCode.A)) TriggerAnger();
        if (Input.GetKeyDown(KeyCode.S)) TriggerSadness();

        // Speed / Intensity test keys
        if (Input.GetKeyDown(KeyCode.UpArrow)) SetIntensity(1.5f);
        if (Input.GetKeyDown(KeyCode.DownArrow)) SetIntensity(0.75f);
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
            UIManager.Instance.UpdateCharacterDisplay(activeChar.characterName, activeChar.characterBio);
            UIManager.Instance.UpdateMoodDisplay("Idle", Color.gray);
        }
    }

    public void TriggerJoy()
    {
        characters[activeIndex].PlayJoy();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayCheerSFX();
        if (UIManager.Instance != null) UIManager.Instance.UpdateMoodDisplay("Joy", new Color(1f, 0.85f, 0.2f));
        Debug.Log("Triggered: Joy");
    }

    public void TriggerAnger()
    {
        characters[activeIndex].PlayAnger();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayAngerSFX();
        if (UIManager.Instance != null) UIManager.Instance.UpdateMoodDisplay("Anger", new Color(0.9f, 0.25f, 0.25f));
        Debug.Log("Triggered: Anger");
    }

    public void TriggerSadness()
    {
        characters[activeIndex].PlaySadness();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayCrySFX();
        if (UIManager.Instance != null) UIManager.Instance.UpdateMoodDisplay("Sadness", new Color(0.25f, 0.5f, 0.9f));
        Debug.Log("Triggered: Sadness");
    }

    public void SetIntensity(float value)
    {
        characters[activeIndex].SetIntensity(value);
        Debug.Log($"Mood Intensity set to: {value}");
    }
}