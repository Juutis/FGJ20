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


    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<WarpEffect>();
        motherShip = GameObject.FindGameObjectWithTag("MotherShip").GetComponent<MotherShip>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToWarp())
        {
            ui.ShowWarpText();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ui.HideWarpText();
                effect.Warp();
                fuel--;
                player.transform.position = motherShip.transform.position;
                player.Disable();
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
}
