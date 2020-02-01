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
        int nowLeft = (int)(timeLeft * 10f);
        if (lastTimeLeft > nowLeft)
        {
            lifeSupportTimer.text = (nowLeft / 10f).ToString("F1");
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
}
