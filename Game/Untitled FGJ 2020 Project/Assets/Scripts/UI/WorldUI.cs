using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUI : MonoBehaviour
{
    public static WorldUI main;
    void Awake()
    {
        main = this;
    }

    private InfoPopup infoPopup;

    private Transform playerTransform;

    void Start()
    {
        infoPopup = GetComponentInChildren<InfoPopup>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void ShowTractorInfo(Transform target)
    {
        ShowInfo("Press z to push\n Press x to pull", target);
    }

    public void HideTractorInfo(Transform target)
    {
        infoPopup.HideIfSameTarget(target);
    }

    private void ShowInfo(string message, Transform target)
    {

        infoPopup.ShowText(message, target);
    }

    public void ShowPlayerInfo()
    {
        ShowInfo("Arrow keys to move.", playerTransform);
        Invoke("ShowShootControls", 5f);
    }

    public void ShowShootControls() {
        ShowInfo("Press space bar to shoot.", playerTransform);
        Invoke("HidePlayerInfo", 5f);
    }

    public void HidePlayerInfo() {
        infoPopup.HideIfSameTarget(playerTransform);
    }
}
