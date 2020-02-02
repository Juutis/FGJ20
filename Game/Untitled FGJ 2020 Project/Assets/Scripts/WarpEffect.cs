using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpEffect : MonoBehaviour
{
    [SerializeField]
    MeshRenderer background;

    SpriteRenderer[] objects; 

    [SerializeField]
    Color warpColor;
    
    List<ParticleSystem> warpEffects = new List<ParticleSystem>();

    [SerializeField]
    float warpDurationStart = 1.0f;
    [SerializeField]
    float warpDurationEnd = 1.0f;

    private Material bgMaterial;
    private Color origBgColorMain, origBgColorSecondary;

    public bool warping = false;
    private float warpTimer = 0;
    private bool warpEffectPlaying = false;

    private float origScaleX = 1.0f;
    private float origScaleY = 1.0f;
    private float scaleX = 1.0f;
    private float scaleY = 1.0f;

    GameObject motherShip;

    float origBgScaleX;

    private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        bgMaterial = background.material;
        origBgColorMain = bgMaterial.GetColor("_MainColor");
        origBgColorSecondary = bgMaterial.GetColor("_SecondaryColor");
        origBgScaleX = bgMaterial.GetFloat("_ScaleX");

        motherShip = GameObject.FindGameObjectWithTag("MotherShip");
        objects = motherShip.GetComponentsInChildren<SpriteRenderer>();

        origScaleX = motherShip.transform.localScale.x;
        origScaleY = motherShip.transform.localScale.y;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        foreach (var obj in GameObject.FindGameObjectsWithTag("WarpEffect"))
        {
            warpEffects.Add(obj.GetComponent<ParticleSystem>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (warpTimer > Time.time)
        {
            if (warping)
            {
                float lerp = 1.0f - (warpTimer - Time.time) / warpDurationStart;
                bgMaterial.SetColor("_MainColor", Color.Lerp(origBgColorMain, warpColor, lerp));
                bgMaterial.SetColor("_SecondaryColor", Color.Lerp(origBgColorSecondary, warpColor, lerp));
                foreach (var o in objects)
                {
                    o.material.SetFloat("_Intensity", 1.0f - lerp);
                }
            } else
            {
                float lerp = 1.0f - (warpTimer - Time.time) / warpDurationEnd;
                bgMaterial.SetColor("_MainColor", Color.Lerp(warpColor, origBgColorMain, lerp));
                bgMaterial.SetColor("_SecondaryColor", Color.Lerp(warpColor, origBgColorSecondary, lerp));

                motherShip.transform.localScale = new Vector3(Mathf.Lerp(scaleX, origScaleX, lerp), Mathf.Lerp(scaleY, origScaleY, lerp), 1.0f);
                bgMaterial.SetFloat("_ScaleX", Mathf.Lerp(bgMaterial.GetFloat("_ScaleX"), origBgScaleX, lerp));
                foreach (var o in objects)
                {
                    o.material.SetFloat("_Intensity", lerp);
                }
            }
        }
        else
        {
            if (warpEffectPlaying)
            {
                if (warping)
                {
                    bgMaterial.SetColor("_MainColor", warpColor);
                    bgMaterial.SetColor("_SecondaryColor", warpColor);
                    foreach (var o in objects)
                    {
                        o.material.SetFloat("_Intensity", 0.0f);
                    }
                }
                else
                {
                    bgMaterial.SetColor("_MainColor", origBgColorMain);
                    bgMaterial.SetColor("_SecondaryColor", origBgColorSecondary);
                    motherShip.transform.localScale = new Vector3(origScaleX, origScaleY, 1.0f);
                    bgMaterial.SetFloat("_ScaleX", origBgScaleX);
                    foreach (var o in objects)
                    {
                        o.material.SetFloat("_Intensity", 1.0f);
                    }
                }
                warpEffectPlaying = false;
            }
        }

        if (warping)
        {
            float scaleTime = Time.time - warpTimer + warpDurationStart;
            scaleX = origScaleX * (Mathf.Log10(scaleTime + 1) + 1);
            scaleY = origScaleY / (Mathf.Log10(scaleTime + 1) + 1);

            motherShip.transform.localScale = new Vector3(scaleX, scaleY, 1.0f);

            bgMaterial.SetFloat("_ScaleX", origBgScaleX * scaleTime);
        }
    }

    public void Warp()
    {
        motherShip.GetComponent<MotherShip>().HyperSpaceStart.Play();

        LevelManager.main.DisableLevel();
        foreach (var effect in warpEffects)
        {
            effect.Play();
        }
        player.transform.position = motherShip.transform.position;
        player.Disable();
        DualMusicPlayer.main.EnterWarp();

        warpTimer = Time.time + warpDurationStart;
        warpEffectPlaying = true;
        warping = true;
        Invoke("ComeOutOfWarp", Random.Range(LevelManager.main.MinWarpLength, LevelManager.main.MaxWarpLength));
    }

    public void ComeOutOfWarp() {
        UnWarp();
        LevelManager.main.StartNextLevel();
    }

    public void UnWarp()
    {
        motherShip.GetComponent<MotherShip>().HyperSpaceEnd.Play();
        foreach (var effect in warpEffects)
        {
            effect.Stop();
        }
        DualMusicPlayer.main.ExitWarp();
        warpTimer = Time.time + warpDurationEnd;
        warpEffectPlaying = true;
        warping = false;
        player.Activate();
    }
}
