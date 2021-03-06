﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private float minWarpLength = 8f;
    [SerializeField]
    private float maxWarpLength = 15f;
    public float MaxWarpLength { get { return maxWarpLength; } }
    public float MinWarpLength { get { return minWarpLength; } }

    [SerializeField]
    private List<GameObject> levels = new List<GameObject>();

    [SerializeField]
    public List<Color> colors = new List<Color>();
    [SerializeField]
    private int currentLevelIndex = 0;

    private GameObject currentLevelObject;

    private Transform player;

    private UI ui;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        foreach (GameObject level in levels)
        {
            level.SetActive(false);
        }
        StartNextLevel();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        WorldUI.main.ShowPlayerInfo();
    }

    public void DisableLevel()
    {
        if (currentLevelObject != null)
        {
            currentLevelObject.SetActive(false);
        }
    }
    public void StartNextLevel()
    {
        if (currentLevelIndex < levels.Count)
        {
            currentLevelObject = levels[currentLevelIndex];
            Transform playerPosition = null;
            foreach (Transform child in currentLevelObject.transform)
            {
                if (child.name == "PlayerPos")
                {
                    playerPosition = child;
                }
            }
            currentLevelObject.SetActive(true);
            player.position = new Vector3(playerPosition.position.x, playerPosition.position.y, player.position.z);
            playerPosition.gameObject.SetActive(false);
            if (currentLevelIndex >= 2) {
                WorldUI.main.ShowShootControls();
            }
            currentLevelIndex += 1;
        } else {
            ui.Win();
        }
    }

    public void ResetPlayerPosition()
    {
        Transform playerPosition = null;
        foreach (Transform child in currentLevelObject.transform)
        {
            if (child.name == "PlayerPos")
            {
                playerPosition = child;
            }
        }
        player.position = new Vector3(playerPosition.position.x, playerPosition.position.y, player.position.z);
    }

    public Color levelColor()
    {
        if (currentLevelIndex < colors.Count) {
            return colors[currentLevelIndex];
        }
        return Color.white;
    }
}
