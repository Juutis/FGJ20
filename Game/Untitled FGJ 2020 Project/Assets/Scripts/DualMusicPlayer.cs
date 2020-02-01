using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualMusicPlayer : MonoBehaviour
{
    public static DualMusicPlayer main;

    [SerializeField]
    private bool muted = true;
    void Awake() {
        main = this;
    }
    [SerializeField]
    AudioSource warpMusic;
    [SerializeField]
    AudioSource normalMusic;

    [SerializeField]
    private float warpVolume = 0.5f;
    [SerializeField]
    private float normalVolume = 0.5f;
    void Start()
    {
        warpMusic.volume = 0f;
        normalMusic.volume = 0f;
        ExitWarp();
    }

    public void EnterWarp(){
        normalMusic.volume = 0f;
        if (!muted) {
            warpMusic.volume = warpVolume;
        }
    }

    public void ExitWarp() {
        warpMusic.volume = 0f;
        if (!muted) {
            normalMusic.volume = normalVolume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
