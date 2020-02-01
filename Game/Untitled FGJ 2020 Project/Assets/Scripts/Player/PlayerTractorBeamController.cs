using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTractorBeamController : MonoBehaviour
{

    private TractorBeam beam;
    void Start()
    {
        beam = GetComponentInChildren<TractorBeam>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            beam.Activate();
            beam.SetType(TractorBeamType.Push);
        } else if (Input.GetKeyDown(KeyCode.O)) {
            beam.Activate();
            beam.SetType(TractorBeamType.Pull);
        } else if (!(Input.GetKey(KeyCode.O) || Input.GetKey(KeyCode.P))) {
            beam.Deactivate();
        }
    }
}
