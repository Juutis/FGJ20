using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    GameObject lifeSupportWarning;

    [SerializeField]
    Text lifeSupportTimer;

    [SerializeField]
    GameObject warpText;

    [SerializeField]
    GameObject warpDamaged;

    [SerializeField]
    GameObject lifeSupportDamaged;

    [SerializeField]
    GameObject outOfFuel;

    int lastTimeLeft = int.MaxValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLifeSupportTimer(float timeLeft)
    {
        int nowLeft = (int)(timeLeft * 100f);
        if (lastTimeLeft > nowLeft)
        {
            lifeSupportTimer.text = nowLeft.ToString() + "%";
            lastTimeLeft = nowLeft;
        }
    }

    public void ShowLifeSupportWarning()
    {
        lastTimeLeft = int.MaxValue;
        lifeSupportWarning.SetActive(true);
    }

    public void HideLifeSupportWarning()
    {
        lifeSupportWarning.SetActive(false);
    }

    public void ShowWarpText()
    {
        warpText.SetActive(true);
    }

    public void HideWarpText()
    {
        warpText.SetActive(false);
    }

    public void ShowWarpDamaged()
    {
        warpDamaged.SetActive(true);
    }

    public void HideWarpDamaged()
    {
        warpDamaged.SetActive(false);
    }

    public void ShowLifeSupportDamaged()
    {
        lifeSupportDamaged.SetActive(true);
    }

    public void HideLifeSupportDamaged()
    {
        lifeSupportDamaged.SetActive(false);
    }

    public void ShowOutOfFuel()
    {
        outOfFuel.SetActive(true);
    }

    public void HideOutOfFuel()
    {
        outOfFuel.SetActive(false);
    }
}
