using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void FadeComplete();
public class FullscreenFade : MonoBehaviour
{

    public static FullscreenFade main;
    private Image imgFade;

    [SerializeField]
    [Range(0f, 20f)]
    private float fadeInDuration;

    [SerializeField]
    [Range(0f, 20f)]
    private float fadeOutDuration;

    [SerializeField]
    [Range(0f, 20f)]
    private float fadeOutIntroDuration;

    [SerializeField]
    private Color fadeInColor = new Color(0, 0, 0, 1);

    [SerializeField]
    private Color fadeOutColor = new Color(0, 0, 0, 0);

    [SerializeField]
    private bool fadeMusic = false;

    private float originalMusicVolume;
    private float targetMusicVolume;

    private float fadingDuration;

    private float fadingTimer;

    private bool isFading = false;


    private Color targetColor;
    private Color originalColor;

    FadeComplete completeCallback;

    MusicPlayer musicPlayer;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        if (GameObject.FindGameObjectsWithTag("FadeManager").Length > 1) {
            Destroy(gameObject);
        }
        main = this;
    }

    void Start()
    {
        imgFade = GetComponentInChildren<Image>();
        musicPlayer  = MusicPlayer.main;
    }

    public void IntroFadeIn() {

    }

    public void Intro() {
        imgFade.color = fadeInColor;
        FadeOut(IntroFadeIn);
        fadingDuration = fadeOutIntroDuration;
    }

    public void FadeInMusic() {
        originalMusicVolume = 0f;
        targetMusicVolume = musicPlayer.GetCurrentMainVolume();
    }

    public void FadeOutMusic() {
        originalMusicVolume = musicPlayer.GetCurrentMainVolume();
        targetMusicVolume = 0f;
    }

    public void FadeIn(FadeComplete fadeCompleteCallback) {
        fadingDuration = fadeInDuration;
        targetColor = fadeInColor;
        completeCallback = fadeCompleteCallback;
        if (fadeMusic) {
            FadeOutMusic();
        }
        StartFading();
    }

    public void FadeOut(FadeComplete fadeCompleteCallback) {
        fadingDuration = fadeOutDuration;
        targetColor = fadeOutColor;
        completeCallback = fadeCompleteCallback;
        if (fadeMusic) {
            FadeInMusic();
        }
        StartFading();
    }

    private void StartFading() {
        originalColor = imgFade.color;
        isFading = true;
        fadingTimer = 0f;
    }

    void Update()
    {
        if (isFading) {
            fadingTimer += Time.unscaledDeltaTime / fadingDuration;
            imgFade.color =  Color.Lerp(originalColor, targetColor, fadingTimer);
            if (fadeMusic) {
                musicPlayer.SetVolume(Mathf.Lerp(originalMusicVolume, targetMusicVolume, fadingTimer));
            }
            if (fadingTimer >= 1) {
                musicPlayer.SetVolume(targetMusicVolume);
                imgFade.color = targetColor;
                fadingTimer = 0f;
                isFading = false;
                completeCallback();
            }
        }
    }
}
