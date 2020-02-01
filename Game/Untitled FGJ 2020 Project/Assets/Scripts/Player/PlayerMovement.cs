using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    GameObject sprite;

    private Rigidbody2D rb2d;


    private GameObject backThruster;
    private GameObject leftNavigationThruster;
    private GameObject rightNavigationThruster;

    private GameObject frontThrusterLeft;
    private GameObject frontThrusterRight;

    private float axisTreshold = 0.05f;

    private PlayerMovementConfig config;

    private bool active = true;
    float horizontalAxis, verticalAxis;

    private enum ShipDirection
    {
        Forward,
        Backward
    }

    private void Start() {
        config = (PlayerMovementConfig) Resources.Load("Configs/PlayerMovementConfig");
        if (config == null) {
            Debug.LogWarning("Couldn't find Configs/PlayerMovementConfig!");
        }
        rb2d = GetComponent<Rigidbody2D>();

        FindThrusters();
    }

    private void FindThrusters() {
        backThruster = FindChildObject("backThruster");
        leftNavigationThruster = FindChildObject("leftNavigationThruster");
        rightNavigationThruster = FindChildObject("rightNavigationThruster");
        frontThrusterLeft = FindChildObject("frontThrusterLeft");
        frontThrusterRight = FindChildObject("frontThrusterRight");
    }

    private GameObject FindChildObject (string name) {
        foreach(Transform child in transform.GetComponentsInChildren<Transform>()) {
            if (child.name == name) {
                return child.gameObject;
            }
        }
        return null;
    }
    
    private void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        if (active)
        {
            rb2d.drag = config.AngularDrag;
            rb2d.angularDrag = config.AngularDrag;
            rb2d.mass = config.Mass;
            HandleHorizontalAxis(horizontalAxis);
        }
    }

    private void FixedUpdate()
    {
        if (!active)
        {
            rb2d.velocity = Vector2.zero;
        }
        else
        {
            HandleVerticalAxis(verticalAxis);
        }
    }

    private void HandleHorizontalAxis(float horizontalAxis) {
        if (Mathf.Abs(horizontalAxis) <= axisTreshold) {
            ActivateNavigationThrusters(false);
        } else {
            if (horizontalAxis > 0f) {
                ActivateLeftNavigationThruster(true);
            } else {
                ActivateRightNavigationThruster(true);
            }
            TurnShip(horizontalAxis);
        }
    }

    private void TurnShip(float horizontalAxis) {
        //rb2d.AddTorque(-horizontalAxis * config.RotationSpeed, config.RotationForceMode2D);
        sprite.transform.Rotate(Vector3.forward, - horizontalAxis * config.RotationSpeed * Time.deltaTime);
    }

    private void HandleVerticalAxis(float verticalAxis) {
        if (Mathf.Abs(verticalAxis) <= axisTreshold) {
            ActivateMainBackThruster(false);
            ActivateFrontThrusters(false);
        } else if (verticalAxis > 0) {
            ActivateMainBackThruster(true);
            ActivateFrontThrusters(false);
            MoveShip(ShipDirection.Forward);
        } else {
            ActivateFrontThrusters(true);
            ActivateMainBackThruster(false);
            MoveShip(ShipDirection.Backward);
        }
    }

    private void MoveShip(ShipDirection direction) {
        if (rb2d.velocity.magnitude < config.VelocityMagnitudeMax) {
            Vector2 vDirection = sprite.transform.up;
            float speed = config.ForwardSpeed;
            if (direction == ShipDirection.Backward) {
                vDirection = -vDirection;
                speed = config.BackwardSpeed;
            }
            rb2d.AddForce(vDirection * speed, config.SpeedForceMode2D);
        }
    }

    private void ActivateMainBackThruster(bool active) {
        backThruster.SetActive(active);
    }

    private void ActivateFrontThrusters(bool active) {
        frontThrusterLeft.SetActive(active);
        frontThrusterRight.SetActive(active);
    }

    private void ActivateNavigationThrusters(bool active) {
        ActivateLeftNavigationThruster(active);
        ActivateRightNavigationThruster(active);
    }

    private void ActivateLeftNavigationThruster(bool active) {
        leftNavigationThruster.SetActive(active);
    }

    private void ActivateRightNavigationThruster(bool active) {
        rightNavigationThruster.SetActive(active);
    }

    public void Activate()
    {
        active = true;
    }

    public void Disable()
    {
        active = false;
    }

}