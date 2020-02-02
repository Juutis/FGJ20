using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartLauncher : MonoBehaviour
{
    private GameObject launchTarget;
    private MotherShip motherShip;
    private bool launched = false;

    void Start()
    {
        motherShip = GameObject.FindGameObjectWithTag("MotherShip").GetComponent<MotherShip>();
        foreach(Transform child in transform)
        {
            if(child.name == "LaunchTarget")
            {
                launchTarget = child.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!launched)
        {
            motherShip.LaunchRandomPartToTarget(launchTarget.transform.position);
            launched = true;
        }
    }
}
