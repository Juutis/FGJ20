using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{

    // Start is called before the first frame update
    private bool isEnteringGame = false;
    void Start()
    {
        MusicPlayer.main.PlayIntroMusic();
        FullscreenFade.main.Intro();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnteringGame && Input.GetKeyDown(KeyCode.Return))
        {
            isEnteringGame = true;
            FullscreenFade.main.FadeIn(FadeInComplete);
        }
    }

    private void FadeInComplete() {
        MusicPlayer.main.PlayGameMusic();
        SceneManager.LoadScene("main");
    }
}
