using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ShipPartType
{
    None,
    LifeSupport,
    Engine
}
public class ShipPart : MonoBehaviour
{
    [SerializeField]
    public bool isLifeSupport = false;

    Rigidbody2D rb;
    Collider2D coll;

    float collisionDisabledUntil;

    MotherShip ship;
    Bounds bounds;

    private float xMinDistanceFromBounds = 10f;
    private float yMinDistanceFromBounds = 8f;

    private SpriteRenderer ghostSpriteRenderer;
    private SpriteRenderer spriteRenderer;

    private bool launchStarted = false;
    private bool launchFinished = false;
    private bool redocking = false;

    private float minRedockDistance = 3.5f;

    private float redockRotateDuration = 0.2f;
    private float redockRotateTimer = 0f;
    private float redockPositionTimer = 0f;
    private float redockPositionDuration = 0.5f;
    private Quaternion dockingRotation;
    private Vector3 dockingPosition;

    [SerializeField]
    private ShipPartType shipPartType;
    private ShipPartType ShipPartType { get { return shipPartType; } }

    private bool isDockable = false;
    public bool IsDockable { get { return isDockable; } set { isDockable = value; } }

    private Color ghostColor = new Color(0.75f, 0.9f, 1, 0.15f);

    public Vector3 OriginalPosition;
    public Quaternion OriginalRotation;

    List<ShipPart> shipParts;

    private ShipPart dockingTarget;

    void Start()
    {
        OriginalPosition = transform.position;
        OriginalRotation = transform.rotation;
        spriteRenderer = GetComponent<SpriteRenderer>();
        CreateGhost();
        HideGhost();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        ship = GameObject.FindGameObjectWithTag("MotherShip").GetComponent<MotherShip>();
    }

    void Update()
    {
        if (!coll.enabled && collisionDisabledUntil < Time.time)
        {
            coll.enabled = true;
        }
        HandleLaunch();
        HandleDocking();
    }

    private void HandleLaunch()
    {
        if (launchStarted && rb.velocity.magnitude > 0.05f)
        {
            bool tooFarToTheRight = transform.position.x >= (bounds.max.x - xMinDistanceFromBounds);
            bool tooFarToTheLeft = transform.position.x <= (bounds.min.x + xMinDistanceFromBounds);
            bool tooFarUp = transform.position.y >= (bounds.max.y - yMinDistanceFromBounds);
            bool tooFarDown = transform.position.y <= (bounds.min.y + yMinDistanceFromBounds);
            if (tooFarToTheRight || tooFarToTheLeft || tooFarUp || tooFarDown)
            {
                rb.velocity = Vector2.zero;
                launchFinished = true;
                launchStarted = false;
            }
        }
        else if (launchStarted && rb.velocity.magnitude < 0.05f)
        {
            launchFinished = true;
        }
    }

    private void HandleDocking()
    {
        if (launchFinished && !redocking)
        {
            foreach (ShipPart shipPart in shipParts)
            {
                if (
                    shipPart.IsDockable &&
                    shipPart.ShipPartType == shipPartType &&
                    Vector2.Distance(transform.position, shipPart.OriginalPosition) <= minRedockDistance
                )
                {
                    dockingTarget = shipPart;
                    Redock();
                    break;
                }
            }
        }
        else if (redocking)
        {
            bool positionIsCorrect = Mathf.Abs(Vector3.Distance(transform.position, dockingTarget.OriginalPosition)) < 0.05f;
            bool rotationIsCorrect = Mathf.Abs(Quaternion.Angle(transform.rotation, dockingTarget.OriginalRotation)) < 0.05f;
            if (positionIsCorrect && rotationIsCorrect)
            {
                redockPositionTimer = 0f;
                redockRotateTimer = 0f;
                Repair();
            }
            else
            {
                if (!positionIsCorrect)
                {
                    redockPositionTimer += Time.deltaTime / redockPositionDuration;
                    transform.position = Vector3.Lerp(dockingPosition, dockingTarget.OriginalPosition, redockPositionTimer);
                }
                if (!rotationIsCorrect)
                {
                    redockRotateTimer += Time.deltaTime / redockRotateDuration;
                    transform.rotation = Quaternion.Lerp(dockingRotation, dockingTarget.OriginalRotation, redockRotateTimer);
                }
            }

        }
    }

    private void Redock()
    {
        dockingRotation = transform.rotation;
        dockingPosition = transform.position;
        redocking = true;
    }

    private void CreateGhost()
    {
        ghostSpriteRenderer = new GameObject(string.Format("Ghost of {0}", name)).AddComponent<SpriteRenderer>();
        ghostSpriteRenderer.sprite = spriteRenderer.sprite;
        ghostSpriteRenderer.color = ghostColor;
        ghostSpriteRenderer.transform.parent = null;
        ghostSpriteRenderer.transform.position = transform.position;
        ghostSpriteRenderer.transform.rotation = transform.rotation;
        ghostSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        ghostSpriteRenderer.transform.localScale = transform.lossyScale;
    }

    public void ShowGhost()
    {
        isDockable = true;
        ghostSpriteRenderer.enabled = true;
    }

    public void HideGhost()
    {
        ghostSpriteRenderer.enabled = false;
    }

    public void Launch(Vector2 force, List<ShipPart> shipParts)
    {
        this.shipParts = shipParts;
        if (dockingTarget == null) {
            ShowGhost();
        } else {
            dockingTarget.ShowGhost();
        }
        launchFinished = false;
        launchStarted = true;
        bounds = Camera.main.GetComponent<FollowCamera>().GetBounds();
        rb.AddForce(force, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-100f, 100f));
        collisionDisabledUntil = Time.time + 3.0f;
        coll.enabled = false;
        rb.simulated = true;
        
    }

    private void Repair() {
        transform.position = dockingTarget.OriginalPosition;
        transform.rotation = dockingTarget.OriginalRotation;
        launchStarted = false;
        redocking = false;
        launchFinished = false;
        dockingTarget.IsDockable = false;
        dockingTarget.HideGhost();
        rb.simulated = false;
        ship.AttachPart(this);
    }
}
