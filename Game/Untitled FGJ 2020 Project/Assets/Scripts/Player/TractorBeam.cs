using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TractorBeamType
{
    None,
    Push,
    Pull
}

[RequireComponent(typeof(LineRenderer))]
public class TractorBeam : MonoBehaviour
{
    // Start is called before the first frame update
    private LineRenderer line;
    private Vector2 offset;

    private float beamLength = 5;
    private float currentLength = 5f;
    private TractorBeamType beamType;
    private SpriteRenderer backgroundBeamSr;

    [SerializeField]
    private float offsetChangeSpeed = 5f;

    [SerializeField]
    private LayerMask targetLayers;

    private float forceStrengthBack = 5f;
    private float forceStrengthFront = 10f;
    private float bufferSize = 0.1f;
    private bool isActive = false;

    private Tractorable currentTractorable;
    void Start()
    {
        FindBeamSpriteRenderers();
        line = GetComponent<LineRenderer>();
        if (!isActive)
        {
            line.enabled = false;
            backgroundBeamSr.enabled = false;
        }

        offset = line.material.GetTextureOffset("_MainTex");
        SetLength(beamLength);
    }

    private void FindBeamSpriteRenderers()
    {
        backgroundBeamSr = FindChildObject("backgroundBeam");
    }

    private SpriteRenderer FindChildObject(string name)
    {
        foreach (Transform child in transform)
        {
            if (child.name == name)
            {
                return child.GetComponent<SpriteRenderer>();
            }
        }
        return null;
    }

    public void Activate()
    {
        isActive = true;
        line.enabled = true;
        backgroundBeamSr.enabled = true;
    }

    public void Deactivate()
    {
        if (isActive)
        {
            isActive = false;
            line.enabled = false;
            backgroundBeamSr.enabled = false;
        }
    }

    public void SetType(TractorBeamType beamType)
    {
        this.beamType = beamType;
        SetLinePosition();
    }

    private void SetLinePosition()
    {
        Vector2 firstPoint = Vector2.zero;
        Vector2 secondPoint = new Vector2(0, currentLength);
        if (beamType == TractorBeamType.Pull)
        {
            firstPoint = secondPoint;
            secondPoint = Vector2.zero;
        }
        line.SetPosition(0, firstPoint);
        line.SetPosition(1, secondPoint);
    }

    public void SetLength(float length)
    {
        currentLength = length;
        SetLinePosition();
        backgroundBeamSr.size = new Vector2(backgroundBeamSr.size.x, currentLength * 2);
    }

    void Update()
    {
        if (isActive)
        {
            float offsetX = offsetChangeSpeed * Time.deltaTime;
            offset.x -= offsetX;
            line.material.SetTextureOffset("_MainTex", offset);

            RaycastHit2D raycastHit2d = Physics2D.Raycast(transform.position, transform.up, beamLength, targetLayers);
            Debug.DrawRay(transform.position, transform.up * beamLength, Color.blue);
            if (raycastHit2d.collider != null)
            {
                //Debug.Log(raycastHit2d.collider.name);
                Tractorable tractorable = raycastHit2d.collider.GetComponent<Tractorable>();
                Vector2 force = (transform.position - tractorable.transform.position).normalized;
                force *= (beamType == TractorBeamType.Push ? forceStrengthFront : forceStrengthBack) * (beamType == TractorBeamType.Push ? -1 : 1);
                if (currentTractorable == tractorable) {
                    tractorable.Tractor(force);
                } else {
                    currentTractorable = tractorable;
                    tractorable.StartTractor(force, beamType);
                }
                currentLength = (transform.position - tractorable.transform.position).magnitude + bufferSize;
                SetLength(currentLength);
            }
            else
            {
                currentLength = beamLength;
                currentTractorable = null;
                if (beamType == TractorBeamType.Push)
                {
                    SetLength(currentLength);
                }

            }
        } else {
            currentTractorable = null;
        }
    }
}
