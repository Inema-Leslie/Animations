using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip ambienceClip;
    [SerializeField] private AudioClip joySFX;
    [SerializeField] private AudioClip angerSFX;
    [SerializeField] private AudioClip sadnessSFX;

    [Header("Animation Sync Settings")]
    [Tooltip("Default duration (in seconds) of your character's reaction clips before cutting SFX.")]
    [SerializeField] private float defaultClipDuration = 2.5f;

    private float masterVolume = 1f;
    private bool isMuted = false;
    private Coroutine playbackSyncCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (bgmSource != null && ambienceClip != null)
        {
            bgmSource.clip = ambienceClip;
            bgmSource.loop = true;
            bgmSource.volume = masterVolume * 0.7f;
            bgmSource.Play();
        }
    }

    
    public void PlayCheerSFX(float speedMultiplier = 1f) => PlayEmotionSFX(joySFX, speedMultiplier);
    public void PlayAngerSFX(float speedMultiplier = 1f) => PlayEmotionSFX(angerSFX, speedMultiplier);
    public void PlayCrySFX(float speedMultiplier = 1f) => PlayEmotionSFX(sadnessSFX, speedMultiplier);

    public void PlayEmotionSFX(AudioClip clip, float speedMultiplier = 1f)
    {
        if (clip == null || sfxSource == null) return;

        
        if (playbackSyncCoroutine != null) StopCoroutine(playbackSyncCoroutine);
        sfxSource.Stop();

        
        sfxSource.clip = clip;
        sfxSource.volume = masterVolume;
        sfxSource.Play();

        
        if (bgmSource != null) bgmSource.Pause();

        
        float safeMultiplier = Mathf.Max(0.1f, speedMultiplier);
        float activeDuration = defaultClipDuration / safeMultiplier;

        
        playbackSyncCoroutine = StartCoroutine(SyncSFXWithAnimationRoutine(activeDuration));
    }

    private IEnumerator SyncSFXWithAnimationRoutine(float activeDuration)
    {
        
        float fadeTime = 0.2f;
        float playDuration = Mathf.Max(0.1f, activeDuration - fadeTime);

        yield return new WaitForSeconds(playDuration);

        
        float startVol = sfxSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            if (sfxSource != null)
                sfxSource.volume = Mathf.Lerp(startVol, 0f, elapsed / fadeTime);
            yield return null;
        }

        StopEmotionSFX();
    }

    public void StopEmotionSFX()
    {
        if (playbackSyncCoroutine != null)
        {
            StopCoroutine(playbackSyncCoroutine);
            playbackSyncCoroutine = null;
        }

        if (sfxSource != null)
        {
            sfxSource.Stop();
            sfxSource.volume = masterVolume;
        }

        
        if (bgmSource != null && !isMuted)
        {
            bgmSource.UnPause();
        }
    }

    
    public void SetVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        float effectiveVolume = Mathf.Pow(masterVolume, 2f);

        if (bgmSource != null) bgmSource.volume = effectiveVolume * 0.7f;
        if (sfxSource != null) sfxSource.volume = effectiveVolume;
    }

    public void ToggleMute(bool muted)
    {
        isMuted = muted;
        AudioListener.pause = isMuted;
    }
}