using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;


    private GameObject backThruster;
    private GameObject leftNavigationThruster;
    private GameObject rightNavigationThruster;

    private GameObject frontThrusterLeft;
    private GameObject frontThrusterRight;
    private GameObject player;

    private PlayerMovementConfig config;
    private Vector3 firstWayPoint;
    private float waypointFoundTime = 0f;
    private float waypointDuration = 3f;
    private bool reverse = false;
    private float reverseStarted;
    private float reverseTime = 2f;
    private List<Vector3> waypoints = new List<Vector3>();
    private int currentWaypointIndex;
    private int nextWaypointIndex;


    private enum ShipDirection
    {
        Forward,
        Backward
    }

    private enum EnemyState
    {
        Patrol,
        Hunt,
        Shoot,
        Assess,
        Dodge
    }

    private EnemyState state;

    private void Start()
    {
        config = (PlayerMovementConfig)Resources.Load("Configs/PlayerMovementConfig");
        if (config == null)
        {
            Debug.LogWarning("Couldn't find Configs/PlayerMovementConfig!");
        }
        rb2d = GetComponent<Rigidbody2D>();
        FindThrusters();
        state = EnemyState.Hunt;
        player = GameObject.FindGameObjectWithTag("Player");
        firstWayPoint = transform.position;
    }

    private void FindThrusters()
    {
        Transform enemy = FindChildObject("enemy", transform).transform;
        backThruster = FindChildObject("backThruster", enemy);
        leftNavigationThruster = FindChildObject("leftNavigationThruster", enemy);
        rightNavigationThruster = FindChildObject("rightNavigationThruster", enemy);
        frontThrusterLeft = FindChildObject("frontThrusterLeft", enemy);
        frontThrusterRight = FindChildObject("frontThrusterRight", enemy);
    }

    private GameObject FindChildObject(string name, Transform transform)
    {
        foreach (Transform child in transform)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 currentWaypoint = waypoints[currentWaypointIndex];
        Vector3 nextWaypoint = waypoints[nextWaypointIndex];
        float distToCurrent = Vector3.Distance(transform.position, currentWaypoint);
        Debug.Log(distToCurrent + ", \t" + currentWaypoint);

        if (distToCurrent < 0.5f)
        {
            currentWaypointIndex = nextWaypointIndex;
            if (currentWaypointIndex + 1 == waypoints.Count)
            {
                nextWaypointIndex = 0;
            }
            else
            {
                nextWaypointIndex++;
            }
        }

        float angleDiff = Vector3.SignedAngle(transform.up, currentWaypoint - transform.position, Vector3.forward);
        if (distToCurrent < 1.5f)
        {
            //rotate towards the next one while moving to this one
            angleDiff = Vector3.SignedAngle(transform.up, nextWaypoint - transform.position, Vector3.forward);
        }

        float rotateDir = angleDiff < 0 ? -1 : angleDiff > 0 ? 1 : 0;
        ActivateLeftNavigationThruster(false);
        ActivateRightNavigationThruster(false);
        if (rotateDir > 0.01f) ActivateRightNavigationThruster(true);
        else if (rotateDir < -0.01f) ActivateLeftNavigationThruster(true);
        float rotateAmount = Mathf.Min(Mathf.Abs(angleDiff), Time.deltaTime * 90);
        transform.Rotate(Vector3.forward, rotateAmount * rotateDir);

        MoveShip(ShipDirection.Forward);

    }

    private void MoveShip(ShipDirection direction)
    {
        if (rb2d.velocity.magnitude < config.VelocityMagnitudeMax)
        {
            Vector2 vDirection = Vector2.up;
            float speed = config.ForwardSpeed * 2f;
            if (direction == ShipDirection.Backward)
            {
                vDirection = -vDirection;
                speed = config.BackwardSpeed;
            }
            rb2d.AddRelativeForce(vDirection * speed, ForceMode2D.Force);
        }
    }

    private void TurnShip(float horizontalAxis)
    {
        rb2d.AddTorque(-horizontalAxis * config.RotationSpeed, ForceMode2D.Force);
    }
    private void ActivateLeftNavigationThruster(bool active)
    {
        leftNavigationThruster.SetActive(active);
    }

    private void ActivateRightNavigationThruster(bool active)
    {
        rightNavigationThruster.SetActive(active);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag != "Player")
        {
            ContactPoint2D point = collision.contacts[0];
            Vector2 dirVec = (point.point - new Vector2(transform.position.x, transform.position.y)).normalized * 2f;
            firstWayPoint = transform.position - new Vector3(dirVec.x, dirVec.y);
            reverse = true;
            reverseStarted = Time.fixedTime;
        }
    }

    public void SetWaypoints(List<Vector3> wp)
    {
        waypoints = wp;
    }
}
