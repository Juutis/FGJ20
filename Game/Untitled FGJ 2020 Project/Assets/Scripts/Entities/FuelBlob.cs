using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBlob : MonoBehaviour
{

    private bool docking = false;
    private FuelDropSpot fuelDropSpot;
    private float minRedockDistance = 4f;

    private float dockingTimer = 0f;

    private float dockingDuration = 0.3f;

    private Vector3 dockingPosition;

    private Rigidbody2D rb2d;
    private CircleCollider2D circleCollider2d;
    private bool alive = true;

    void Start()
    {
        circleCollider2d = GetComponent<CircleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        fuelDropSpot = GameObject.FindWithTag("MotherShip").GetComponent<MotherShip>().FuelDropSpot;
    }

    void Update()
    {
        HandleDocking();
    }


    private void HandleDocking()
    {
        if (!alive) {
            return;
        }
        if (!docking)
        {
            if (Vector2.Distance(transform.position, fuelDropSpot.transform.position) <= minRedockDistance)
            {
                StartDocking();
            }
        }
        else
        {
            bool positionIsCorrect = Mathf.Abs(Vector3.Distance(transform.position, fuelDropSpot.transform.position)) < 0.05f;
            if (positionIsCorrect)
            {
                FinishDocking();
                Consume();
            }
            else
            {
                dockingTimer += Time.deltaTime / dockingDuration;
                Vector3 lerpedPosition = Vector3.Lerp(dockingPosition, fuelDropSpot.transform.position, dockingTimer);
                lerpedPosition.z = transform.position.z;
                transform.position = lerpedPosition;
            }
        }
    }

    private void StartDocking()
    {
        dockingPosition = transform.position;
        docking = true;
        circleCollider2d.enabled = false;
        rb2d.simulated = false;
    }

    private void FinishDocking() {
        Vector3 lerpedPosition = fuelDropSpot.transform.position;
        lerpedPosition.z = transform.position.z;
        transform.position = lerpedPosition;
        dockingTimer = 0f;
    }

    private void Consume()
    {
        alive = false;
        WarpDrive warpDrive = GameObject.FindGameObjectWithTag("WarpDrive").GetComponent<WarpDrive>();
        warpDrive.GetFuel();
        Invoke("Kill", 0.5f);
    }

    private void Kill() {
        Destroy(gameObject);
    }
}
