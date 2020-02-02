using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTractorBeamController : MonoBehaviour
{

    private TractorBeam beam;
    private KeyCode pushKey = KeyCode.Z;
    private KeyCode pullKey = KeyCode.X;
    void Start()
    {
        beam = GetComponentInChildren<TractorBeam>();
    }

    void Update()
    {
        if (Input.GetKeyDown(pushKey)) {
            beam.Activate();
            beam.SetType(TractorBeamType.Push);
        } else if (Input.GetKeyDown(pullKey)) {
            beam.Activate();
            beam.SetType(TractorBeamType.Pull);
        } else if (!(Input.GetKey(pullKey) || Input.GetKey(pushKey))) {
            beam.Deactivate();
        }
    }
}
