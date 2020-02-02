using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    void Awake() {
        main = this;
    }

    [SerializeField]
    private List<GameObject> levels = new List<GameObject>();
    [SerializeField]
    private int currentLevelIndex =0;

    private GameObject currentLevelObject;
    
    private Transform player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        foreach(GameObject level in levels) {
            level.SetActive(false);
        }
        StartNextLevel();
    }

    public void StartNextLevel() {
        if (currentLevelObject != null) {
            currentLevelObject.SetActive(false);
        }
        if (currentLevelIndex < levels.Count) {
            currentLevelObject = levels[currentLevelIndex];
            Transform playerPosition = null;
            foreach(Transform child in currentLevelObject.transform) {
                if (child.name == "PlayerPos") {
                    playerPosition = child;
                }
            }
            currentLevelObject.SetActive(true);
            player.position = new Vector3(playerPosition.position.x, playerPosition.position.y, player.position.z);
            playerPosition.gameObject.SetActive(false);
            currentLevelIndex += 1;
        } else {
            Debug.Log("The end!");
        }
    }
}
