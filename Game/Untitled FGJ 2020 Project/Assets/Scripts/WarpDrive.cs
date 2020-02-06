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

    private FuelDropSpot fuelDropSpot;


    void Start()
    {
        effect = GetComponent<WarpEffect>();
        motherShip = GameObject.FindGameObjectWithTag("MotherShip").GetComponent<MotherShip>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        levelRoot = GameObject.FindGameObjectWithTag("Level");
        fuelDropSpot = motherShip.FuelDropSpot;
        DisableFuelDropSpot();
    }

    void Update()
    {
        if (UpdateWarpStatus())
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Warp();
            }
        }
    }

    private void ActivateFuelDropSpot(bool active)
    {
        fuelDropSpot.gameObject.SetActive(active);
    }
    private bool UpdateWarpStatus()
    {
        if (effect.warping)
        {
            ui.HideWarpText();
            ui.HideOutOfFuel();
            ui.HideWarpDamaged();
            DisableFuelDropSpot();
            return false;
        }
        if (motherShip.IsReadyToWarp())
        {
            if (fuel > 0)
            {
                ui.ShowWarpText();
                ui.HideOutOfFuel();
                ui.HideWarpDamaged();
                DisableFuelDropSpot();
                return true;
            }
            else
            {
                ui.HideWarpText();
                ui.ShowOutOfFuel();
                ui.HideWarpDamaged();
                ActivateFuelDropSpot(true);
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

    public void GetFuel()
    {
        Invoke("DisableFuelDropSpot", 0.5f);
        fuel = 1;
    }

    private void DisableFuelDropSpot()
    {
        ActivateFuelDropSpot(false);
    }

    private bool ReadyToWarp()
    {
        return motherShip.IsReadyToWarp() && fuel > 0;
    }

    private void Warp()
    {

        ui.HideWarpText();
        effect.Warp();
        fuel--;

        foreach (var part in motherShip.ShipParts)
        {
            part.HideGhost();
            if (!motherShip.AvailableParts.Contains(part))
            {
                part.gameObject.SetActive(false);
            }
        }
        if (levelRoot != null)
        {
            levelRoot.SetActive(false);
        }
    }

}
