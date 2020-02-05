using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicType
{
    Main,
    Secondary
}

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer main;

    [SerializeField]
    private bool muted = true;
    void Awake()
    {
        main = this;
    }


    [SerializeField]
    private MusicSource introMusic;

    [SerializeField]
    private List<MusicSource> musicSources;
    private MusicSource currentMusic;
    private MusicType state = MusicType.Main;

    private AudioSource fadingInMusic;
    private AudioSource fadingOutMusic;

    private bool initialized = false;
    private float mainTimer;
    private float mainDuration;
    private float mainOriginalVolume;
    private float mainTargetVolume;
    private float secondaryTimer;
    private float secondaryDuration;
    private float secondaryOriginalVolume;
    private float secondaryTargetVolume;

    private bool isCrossFading = false;

    public void Initialize()
    {
        if (!initialized)
        {
            foreach (MusicSource musicSource in musicSources)
            {
                musicSource.MainAudioSource = CreateAudioSource(musicSource.MainAudioClip);
                if (musicSource.SecondaryAudioClip != null)
                {
                    musicSource.SecondaryAudioSource = CreateAudioSource(musicSource.SecondaryAudioClip);
                }
            }
            introMusic.MainAudioSource = CreateAudioSource(introMusic.MainAudioClip);
            initialized = true;
        }
    }

    public void PlayIntroMusic()
    {
        Initialize();
        currentMusic = introMusic;
        currentMusic.MainAudioSource.Play();
        
    }
    public void PlayGameMusic()
    {
        currentMusic = musicSources[0];
        currentMusic.MainAudioSource.Play();
        currentMusic.SecondaryAudioSource.Play();
    }

    private AudioSource CreateAudioSource(AudioClip clip)
    {
        AudioSource newAudioSource = new GameObject(clip.name).AddComponent<AudioSource>();
        newAudioSource.clip = clip;
        newAudioSource.volume = 0f;
        newAudioSource.transform.SetParent(transform);
        newAudioSource.loop = true;
        newAudioSource.playOnAwake = false;
        return newAudioSource;
    }

    public float GetCurrentMainVolume()
    {
        if (currentMusic != null)
        {
            return currentMusic.Volume;
        }
        return 0f;
    }

    public void SetMusicState(MusicType musicType)
    {
        state = musicType;
    }

    public void SetVolume(float volume)
    {
        if (!muted)
        {
            if (state == MusicType.Main)
            {
                currentMusic.MainAudioSource.volume = volume;
            }
            else if (state == MusicType.Secondary)
            {
                currentMusic.SecondaryAudioSource.volume = volume;
            }
        }
    }

    private void CrossFade()
    {
        Debug.Log("CrossFade!");
        secondaryTimer = 0f;
        mainTimer = 0f;
        if (state == MusicType.Main) {
            mainDuration = currentMusic.MainMusicFadeOutDuration;
            secondaryDuration = currentMusic.FadeInDuration;
            mainTargetVolume = 0f;
            secondaryTargetVolume = currentMusic.Volume;
        } else {
            secondaryDuration = currentMusic.SecondaryMusicFadeOutDuration;
            mainDuration = currentMusic.FadeInDuration;
            mainTargetVolume = currentMusic.Volume;
            secondaryTargetVolume = 0f;
        }
        mainOriginalVolume = currentMusic.MainAudioSource.volume;
        secondaryOriginalVolume = currentMusic.SecondaryAudioSource.volume;
        isCrossFading = true;
    }

    void Update()
    {
        if (isCrossFading) {
            if (mainTimer < 1) {
                mainTimer += Time.unscaledDeltaTime / mainDuration;
                currentMusic.MainAudioSource.volume = Mathf.Lerp(mainOriginalVolume, mainTargetVolume, mainTimer);
            }
            if (secondaryTimer < 1) {
                secondaryTimer += Time.unscaledDeltaTime / secondaryDuration;
                currentMusic.SecondaryAudioSource.volume = Mathf.Lerp(secondaryOriginalVolume, secondaryTargetVolume, secondaryTimer);
            }
            if (mainTimer >= 1 && secondaryTimer >= 1) {
                isCrossFading = false;
                currentMusic.MainAudioSource.volume = mainTargetVolume;
                currentMusic.SecondaryAudioSource.volume = secondaryTargetVolume;

                if (state == MusicType.Main) {
                    state = MusicType.Secondary;
                } else {
                    state = MusicType.Main;
                }
            }
        }
    }

    public void EnterWarp()
    {
        if (!muted)
        {
            CrossFade();
        }
    }

    public void ExitWarp(bool isIntro)
    {
        if (!muted)
        {
            if (!isIntro) {
                CrossFade();
            }
        }
    }


}

[System.Serializable]
public class MusicSource
{
    [SerializeField]
    private float volume;
    public float Volume { get { return volume; } }

    [SerializeField]
    [Range(0, 25f)]
    private float fadeInDuration;
    public float FadeInDuration { get { return fadeInDuration; } }


    [SerializeField]
    private AudioClip mainAudioclip;
    public AudioClip MainAudioClip { get { return mainAudioclip; } }

    [HideInInspector]
    public AudioSource MainAudioSource;


    [SerializeField]
    private AudioClip secondaryAudioClip;
    public AudioClip SecondaryAudioClip { get { return secondaryAudioClip; } }

    [HideInInspector]
    public AudioSource SecondaryAudioSource;


    [SerializeField]
    [Range(0, 25f)]
    private float mainMusicFadeOutDuration;
    public float MainMusicFadeOutDuration { get { return mainMusicFadeOutDuration; } }

    [SerializeField]
    [Range(0, 25f)]
    private float secondaryMusicFadeOutDuration;
    public float SecondaryMusicFadeOutDuration { get { return secondaryMusicFadeOutDuration; } }

}