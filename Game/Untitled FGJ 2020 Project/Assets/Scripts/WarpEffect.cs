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

    private bool warp = false;
    private float warpTimer = 0;
    private bool warpEffectPlaying = false;

    private float scaleX = 1.0f;
    private float scaleY = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        bgMaterial = background.material;
        origBgColorMain = bgMaterial.GetColor("_MainColor");
        origBgColorSecondary = bgMaterial.GetColor("_SecondaryColor");

        GameObject motherShip = GameObject.FindGameObjectWithTag("MotherShip");
        objects = motherShip.GetComponentsInChildren<SpriteRenderer>();

        foreach (var obj in GameObject.FindGameObjectsWithTag("WarpEffect"))
        {
            warpEffects.Add(obj.GetComponent<ParticleSystem>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (warp)
            {
                UnWarp();
            } else
            {
                Warp();
            }
            warp = !warp;
        }

        if (warpTimer > Time.time)
        {
            if (warp)
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
                foreach (var o in objects)
                {
                    o.material.SetFloat("_Intensity", lerp);
                    o.transform.localScale = new Vector3(Mathf.Lerp(scaleX, 1.0f, lerp), Mathf.Lerp(scaleY, 1.0f, lerp), 1.0f);
                }
            }
        }
        else
        {
            if (warpEffectPlaying)
            {
                if (warp)
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
                    foreach (var o in objects)
                    {
                        o.material.SetFloat("_Intensity", 1.0f);
                        o.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                }
                warpEffectPlaying = false;
            }
        }

        if (warp)
        {
            float scaleTime = Time.time - warpTimer + warpDurationStart;
            //scaleX = 1.25f - 0.25f*Mathf.Cos(scaleTime * 2);
            //scaleY = 0.9f + 0.1f*Mathf.Cos(-scaleTime * 2);
            scaleX = Mathf.Log10(scaleTime + 1) + 1;
            scaleY = 1 / (Mathf.Log10(scaleTime + 1) + 1);

            foreach (var o in objects)
            {
                o.transform.localScale = new Vector3(scaleX, scaleY, 1.0f);
            }
        }
    }

    public void Warp()
    {

        foreach (var effect in warpEffects)
        {
            effect.Play();
        }

        warpTimer = Time.time + warpDurationStart;
        warpEffectPlaying = true;
    }

    public void UnWarp()
    {

        foreach (var effect in warpEffects)
        {
            effect.Stop();
        }

        warpTimer = Time.time + warpDurationEnd;
        warpEffectPlaying = true;
    }
}
