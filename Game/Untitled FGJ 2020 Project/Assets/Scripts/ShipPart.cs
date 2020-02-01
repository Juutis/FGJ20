using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPart : MonoBehaviour
{
    [SerializeField]
    public bool isLifeSupport = false;

    Rigidbody2D rb;
    Collider2D coll;

    float collisionDisabledUntil;

    Vector3 startPos;
    Quaternion startRotation;

    MotherShip ship;
    Bounds bounds;

    private float xMinDistanceFromBounds = 5f;
    private float yMinDistanceFromBounds = 5f;

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
        if (rb.velocity.magnitude > 0.05f) {
            bool tooFarToTheRight = transform.position.x >= (bounds.max.x - xMinDistanceFromBounds);
            bool tooFarToTheLeft = transform.position.x <= (bounds.min.x + xMinDistanceFromBounds);
            bool tooFarUp = transform.position.y >= (bounds.max.y - yMinDistanceFromBounds);
            bool tooFarDown = transform.position.y <= (bounds.min.y + yMinDistanceFromBounds);
            if (tooFarToTheRight || tooFarToTheLeft || tooFarUp || tooFarDown) {
                rb.velocity = Vector2.zero;
            }
        }

    }

    public void Launch(Vector2 force)
    {
        bounds = Camera.main.GetComponent<FollowCamera>().GetBounds();
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
