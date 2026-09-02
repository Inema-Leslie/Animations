using UnityEngine;


public class CharacterMoodController : MonoBehaviour
{
    [Header("Character Identity")]
    public string characterName = "Character";
    [TextArea(2, 4)]
    public string characterBio = "A brief description of this character.";

    private Animator animator;

    
    private static readonly int HashJoy = Animator.StringToHash("TriggerJoy");
    private static readonly int HashAnger = Animator.StringToHash("TriggerAnger");
    private static readonly int HashSadness = Animator.StringToHash("TriggerSadness");
    private static readonly int HashIntensity = Animator.StringToHash("MoodIntensity");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayJoy() => animator.SetTrigger(HashJoy);
    public void PlayAnger() => animator.SetTrigger(HashAnger);
    public void PlaySadness() => animator.SetTrigger(HashSadness);

    public void SetIntensity(float intensity)
    {
        animator.SetFloat(HashIntensity, Mathf.Clamp(intensity, 0.2f, 2.5f));
    }
}