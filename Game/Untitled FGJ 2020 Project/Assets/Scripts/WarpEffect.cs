using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpEffect : MonoBehaviour
{
    [SerializeField]
    MeshRenderer background;

    [SerializeField]
    SpriteRenderer[] objects; 

    [SerializeField]
    Color warpColor;

    [SerializeField]
    ParticleSystem[] warpEffects;

    [SerializeField]
    float warpDurationStart = 1.0f;
    [SerializeField]
    float warpDurationEnd = 1.0f;

    private Material bgMaterial;
    private Color origBgColor;

    private bool warp = false;
    private float warpTimer = 0;
    private bool warpEffectPlaying = false;

    private float scaleX = 1.0f;
    private float scaleY = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        bgMaterial = background.material;
        origBgColor = bgMaterial.color;
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
                bgMaterial.color = Color.Lerp(origBgColor, warpColor, lerp);
                foreach (var o in objects)
                {
                    o.material.SetFloat("_Intensity", 1.0f - lerp);
                }
            } else
            {
                float lerp = 1.0f - (warpTimer - Time.time) / warpDurationEnd;
                bgMaterial.color = Color.Lerp(warpColor, origBgColor, lerp);
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
                    bgMaterial.color = warpColor;
                    foreach (var o in objects)
                    {
                        o.material.SetFloat("_Intensity", 0.0f);
                    }
                }
                else
                {
                    bgMaterial.color = origBgColor;
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
        //bgMaterial.DisableKeyword("_EMISSION");
        bgMaterial.color = warpColor;

        foreach (var o in objects)
        {
            o.material.SetFloat("_Intensity", 0f);
        }

        foreach (var effect in warpEffects)
        {
            effect.Play();
        }

        warpTimer = Time.time + warpDurationStart;
        warpEffectPlaying = true;
    }

    public void UnWarp()
    {
        //bgMaterial.EnableKeyword("_EMISSION");
        background.material.color = origBgColor;
        foreach (var o in objects)
        {
            o.material.SetFloat("_Intensity", 1f);
        }

        foreach (var effect in warpEffects)
        {
            effect.Stop();
        }

        warpTimer = Time.time + warpDurationEnd;
        warpEffectPlaying = true;
    }
}
