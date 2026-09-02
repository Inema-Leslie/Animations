using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip cheerClip;
    [SerializeField] private AudioClip angerClip;
    [SerializeField] private AudioClip cryClip;

    private bool isMuted = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void PlayCheerSFX() => PlaySFX(cheerClip);
    public void PlayAngerSFX() => PlaySFX(angerClip);
    public void PlayCrySFX() => PlaySFX(cryClip);

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null || isMuted) return;
        sfxSource.Stop(); 
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void SetMasterVolume(float value)
    {
        AudioListener.volume = isMuted ? 0f : value;
    }

    public void SetBGMVolume(float value)
    {
        if (bgmSource != null) bgmSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        if (sfxSource != null) sfxSource.volume = value;
    }

    public void ToggleMute(bool mute)
    {
        isMuted = mute;
        AudioListener.pause = isMuted;
    }
}