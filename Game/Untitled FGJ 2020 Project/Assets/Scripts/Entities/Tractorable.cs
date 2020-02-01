using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tractorable : MonoBehaviour
{
    private float maxMagnitude = 8f;
    Rigidbody2D rb2d;

    TractorBeamType previousBeamType = TractorBeamType.None;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    public void Tractor(Vector2 force) {
        if (rb2d.velocity.magnitude < maxMagnitude) {
            rb2d.AddForce(force, ForceMode2D.Force);
        }
    }

    public void StartTractor(Vector2 force, TractorBeamType beamType) {
        if (beamType != previousBeamType) {
            rb2d.velocity = rb2d.velocity * 0.4f;
        }
        previousBeamType = beamType;
        Tractor(force);
    }
}
