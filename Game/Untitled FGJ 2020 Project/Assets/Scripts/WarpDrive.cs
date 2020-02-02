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
        if (updateWarpStatus())
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                warp();
            }
        }
    }

    bool updateWarpStatus()
    {
        if (effect.warping)
        {
            ui.HideWarpText();
            ui.HideOutOfFuel();
            ui.HideWarpDamaged();
            return false;
        }
        if (motherShip.IsReadyToWarp())
        {
            if (fuel > 0)
            {
                ui.ShowWarpText();
                ui.HideOutOfFuel();
                ui.HideWarpDamaged();
                return true;
            }
            else
            {
                ui.HideWarpText();
                ui.ShowOutOfFuel();
                ui.HideWarpDamaged();
                return false;
            }
        }
        else
        {
            ui.HideWarpText();
            ui.HideOutOfFuel();
            ui.ShowWarpDamaged();
            return false;
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
            if (!motherShip.availableParts.Contains(part))
            {
                part.gameObject.SetActive(false);
            }
        }
        levelRoot.SetActive(false);
    }
}
