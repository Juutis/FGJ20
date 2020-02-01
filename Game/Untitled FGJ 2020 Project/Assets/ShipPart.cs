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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        startPos = transform.position;
        startRotation = transform.rotation;
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
        collisionDisabledUntil = Time.time + 3.0f;
        coll.enabled = false;
        rb.simulated = true;
    }

    public void Repair()
    {
        transform.position = startPos;
        transform.rotation = startRotation;
        rb.simulated = false;
    }
}
