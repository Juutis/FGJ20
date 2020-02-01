using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpDrive : MonoBehaviour
{
    WarpEffect effect;
    MotherShip motherShip;
    UI ui;
    PlayerMovement player;

    [SerializeField]
    public int fuel = 1;

    GameObject levelRoot;


    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<WarpEffect>();
        motherShip = GameObject.FindGameObjectWithTag("MotherShip").GetComponent<MotherShip>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        levelRoot = GameObject.FindGameObjectWithTag("Level");
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToWarp())
        {
            ui.ShowWarpText();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                warp();
            }
        }
        else
        {
            ui.HideWarpText();
        }
    }

    bool readyToWarp()
    {
        return motherShip.IsReadyToWarp() && fuel > 0;
    }

    void warp()
    {

        ui.HideWarpText();
        effect.Warp();
        fuel--;
        player.transform.position = motherShip.transform.position;
        player.Disable();
        foreach (var part in motherShip.shipParts)
        {
            part.HideGhost();
            if (part.IsDockable)
            {
                part.gameObject.SetActive(false);
            }
        }
        levelRoot.SetActive(false);
    }
}
