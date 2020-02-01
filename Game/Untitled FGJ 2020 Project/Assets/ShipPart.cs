using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPart : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D coll;

    float collisionDisabledUntil;

    Vector3 startPos;
    Quaternion startRotation;

    MotherShip ship;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        startPos = transform.position;
        startRotation = transform.rotation;
        ship = GameObject.FindGameObjectWithTag("MotherShip").GetComponent<MotherShip>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!coll.enabled && collisionDisabledUntil < Time.time)
        {
            coll.enabled = true;
        }
    }

    public void Launch(Vector2 force)
    {
        rb.AddForce(force);
        rb.AddTorque(Random.Range(-100f, 100f));
        collisionDisabledUntil = Time.time + 3.0f;
        coll.enabled = false;
        rb.simulated = true;
    }

    public void Repair()
    {
        transform.position = startPos;
        transform.rotation = startRotation;
        rb.simulated = false;
        ship.AttachPart(this);
    }
}
