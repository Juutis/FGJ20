using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum FollowCameraZoomState
{
    ZoomedIn,
    ZoomedOut
}
public class FollowCameraZoomer : MonoBehaviour
{
    private Transform target;
    private List<Transform> zoomOutTriggers;

    [SerializeField]
    private float zoomTriggerDistance = 5f;

    private float triggerCheckInterval = 0.5f;
    private float triggerCheckTimer = 0f;

    private Camera targetCamera;

    [SerializeField]
    [Range(1f, 15f)]
    private float zoomSize = 10;

    private float originalZoom;
    private float startingZoom;
    private float targetZoom;
    private float zoomTimer = 0;

    [SerializeField]
    private float zoomDuration = 2.5f;
    private FollowCameraZoomState zoomState = FollowCameraZoomState.ZoomedIn;

    private bool isZooming = false;

    void Start()
    {
        FindZoomOutTriggers();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetCamera = GetComponent<Camera>();
        originalZoom = targetCamera.orthographicSize;
    }

    void FindZoomOutTriggers() {
        zoomOutTriggers = new List<Transform>();
        foreach(GameObject zoomOutTrigger in GameObject.FindGameObjectsWithTag("ZoomOutTrigger")) {
            zoomOutTriggers.Add(zoomOutTrigger.transform);
        }
    }

    void Update()
    {
        if (isZooming)
        {
            LerpZoom();
        }
        else {
            triggerCheckTimer += Time.deltaTime;
            if (triggerCheckTimer > triggerCheckInterval) {
                CheckZoomTriggers();
                triggerCheckTimer = 0f;
            }
        }
    }

    private void CheckZoomTriggers() {
        bool nearAZoomOutTrigger = false;
        foreach(Transform zoomOutTrigger in zoomOutTriggers) {
            if(Vector2.Distance(target.position, zoomOutTrigger.position) <= zoomTriggerDistance) {
                nearAZoomOutTrigger = true;
                break;
            }
        }
        if (nearAZoomOutTrigger) {
            if (zoomState == FollowCameraZoomState.ZoomedIn) {
                ZoomOut();
            }
        } else if (zoomState == FollowCameraZoomState.ZoomedOut) {
            ZoomIn();
        }

    }

    private void LerpZoom()
    {
        zoomTimer += Time.deltaTime / zoomDuration;
        float lerp = Mathf.Lerp(startingZoom, targetZoom, zoomTimer);
        targetCamera.orthographicSize = lerp;
        targetCamera.fieldOfView = lerp * 6;
        if (Mathf.Abs(targetCamera.orthographicSize - targetZoom) < 0.05f)
        {
            targetCamera.orthographicSize = targetZoom;
            targetCamera.fieldOfView = targetZoom * 6;
            isZooming = false;
            zoomTimer = 0f;
        }
    }

    private void StartZooming()
    {
        startingZoom = targetCamera.orthographicSize;
        if (zoomState == FollowCameraZoomState.ZoomedIn)
        {
            targetZoom = startingZoom - zoomSize;
        }
        else
        {
            targetZoom = startingZoom + zoomSize;
        }
        isZooming = true;
    }

    public void ZoomIn()
    {
        zoomState = FollowCameraZoomState.ZoomedIn;
        StartZooming();
    }

    public void ZoomOut()
    {
        zoomState = FollowCameraZoomState.ZoomedOut;
        StartZooming();
    }
}
