using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tractorable : MonoBehaviour
{
    private float maxMagnitude = 8f;
    Rigidbody2D rb2d;

    TractorBeamType previousBeamType = TractorBeamType.None;

    private float minProximityForInfo = 5f;
    private Transform playerTransform;
    private bool showingInfo = false;
    private bool activated = false;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (activated)
        {
            CheckPlayerProximity();
        }
    }

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
        HideInfo();
    }

    public void Tractor(Vector2 force)
    {
        if (rb2d.velocity.magnitude < maxMagnitude)
        {
            rb2d.AddForce(force, ForceMode2D.Force);
        }
    }

    public void StartTractor(Vector2 force, TractorBeamType beamType)
    {
        if (beamType != previousBeamType)
        {
            rb2d.velocity = rb2d.velocity * 0.4f;
        }
        previousBeamType = beamType;
        Tractor(force);
    }

    private void CheckPlayerProximity()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < minProximityForInfo)
        {
            ShowInfo();
        }
        else if (showingInfo)
        {
            HideInfo();
        }
    }

    private void ShowInfo()
    {
        WorldUI.main.ShowTractorInfo(transform);
        showingInfo = true;
    }

    private void HideInfo()
    {
        WorldUI.main.HideTractorInfo(transform);
        showingInfo = true;
    }
}
