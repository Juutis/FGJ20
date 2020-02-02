using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform target;
    private Vector3 newPosition;

    private Vector3 currentVelocity;
    private SpriteRenderer clampedAreaRenderer;
    private Bounds clampedAreaBounds;

    [SerializeField]
    private bool followX = false;
    [SerializeField]
    private bool followY = false;
    [SerializeField]
    private bool hideClampedArea = true;

    private bool canFollow = false;

    [SerializeField]
    [Range(0f, 5f)]
    private float smoothing = 0.5f;

    private string targetTag = "Player";
    private string clampedAreaTag = "ClampedArea";
    float aspectRatio = 1.0f * Screen.width / Screen.height;

    private enum CameraAxis {
        X,
        Y
    }

    private void Awake()
    {
        FindTarget();
        FindClampedArea();
    }

    void Start()
    {
        if (target != null) {
            canFollow = true;
        } else {
            Debug.LogWarning(string.Format("Camera couldn't find target ({0})!", targetTag));
        }
    }

    private void FindTarget() {
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
    }

    private void FindClampedArea() {
        clampedAreaRenderer = GameObject
            .FindGameObjectWithTag(clampedAreaTag)
            .GetComponent<SpriteRenderer>();
        clampedAreaBounds = clampedAreaRenderer.sprite.bounds;
        clampedAreaRenderer.enabled = !hideClampedArea;
    }

    public Bounds GetBounds () {
        Bounds newBounds = new Bounds();
        newBounds.max = new Vector3(
            clampedAreaBounds.max.x * clampedAreaRenderer.transform.localScale.x,
            clampedAreaBounds.max.y * clampedAreaRenderer.transform.localScale.y,
            clampedAreaBounds.max.z * clampedAreaRenderer.transform.localScale.z
        );
        newBounds.min = new Vector3(
            clampedAreaBounds.min.x * clampedAreaRenderer.transform.localScale.x,
            clampedAreaBounds.min.y * clampedAreaRenderer.transform.localScale.y,
            clampedAreaBounds.min.z * clampedAreaRenderer.transform.localScale.z
        );
        return newBounds;
    }
    void Update()
    {
        if (!canFollow) {
            return;
        }
        newPosition = transform.position;
        if (followX) {
            newPosition.x = GetClampedCoordinate(CameraAxis.X);
        }
        if (followY) {
            newPosition.y = GetClampedCoordinate(CameraAxis.Y);
        }
        
        transform.position = Vector3.SmoothDamp(
            transform.position,
            newPosition,
            ref currentVelocity,
            smoothing
        );
    }

    private float GetClampedCoordinate(CameraAxis axis) {
        float targetCoordinate = target.position.x;
        
        float halfOfScreen = Camera.main.orthographicSize * aspectRatio;
        
        float clampMin = clampedAreaBounds.min.x * clampedAreaRenderer.transform.localScale.x;
        float clampMax = clampedAreaBounds.max.x * clampedAreaRenderer.transform.localScale.x;
        
        if (axis == CameraAxis.Y) {
            halfOfScreen = Camera.main.orthographicSize;
            targetCoordinate = target.position.y;
            clampMin = clampedAreaBounds.min.y * clampedAreaRenderer.transform.localScale.y;
            clampMax = clampedAreaBounds.max.y * clampedAreaRenderer.transform.localScale.y;
        }
        
        return Mathf.Clamp(targetCoordinate, halfOfScreen + clampMin, clampMax - halfOfScreen);
    }
}
