using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
    private Rigidbody2D rb2d;


    private GameObject backThruster;
    private GameObject leftNavigationThruster;
    private GameObject rightNavigationThruster;

    private GameObject frontThrusterLeft;
    private GameObject frontThrusterRight;

    private float axisTreshold = 0.05f;

    [SerializeField]
    [Range(1f, 20f)]
    private float rotationSpeed = 5f;

    [SerializeField]
    [Range(1f, 20f)]
    private float velocityMagnitudeMax  = 5f;

    [SerializeField]
    [Range(1f, 20f)]
    private float forwardSpeed = 5f;

    [SerializeField]
    [Range(1f, 20f)]
    private float backwardSpeed = 2.5f;

    private enum ShipDirection
    {
        Forward,
        Backward
    }

    private void Start() {
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
        foreach(Transform child in transform) {
            if (child.name == name) {
                return child.gameObject;
            }
        }
        return null;
    }

    private void Update() {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        HandleHorizontalAxis(horizontalAxis);
        HandleVerticalAxis(verticalAxis);
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
        rb2d.AddTorque(-horizontalAxis * rotationSpeed, ForceMode2D.Force);
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
        if (rb2d.velocity.magnitude < velocityMagnitudeMax) {
            Vector2 vDirection = Vector2.up;
            float speed = forwardSpeed;
            if (direction == ShipDirection.Backward) {
                vDirection = -vDirection;
                speed = backwardSpeed;
            }
            rb2d.AddRelativeForce(vDirection * speed, ForceMode2D.Force);
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

}