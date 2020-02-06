using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MotherShip : MonoBehaviour
{
    [SerializeField]
    private float healthPerModule = 10f;

    [SerializeField]
    private float lifeSupportTime = 60f;

    private float health;

    private List<ShipPart> shipParts = new List<ShipPart>();
    private List<ShipPart> availableParts = new List<ShipPart>();

    public List<ShipPart> ShipParts { get { return shipParts; } }
    public List<ShipPart> AvailableParts { get { return availableParts; } }

    [SerializeField]
    private FuelDropSpot dropSpot;
    public FuelDropSpot FuelDropSpot { get { return dropSpot; } }

    [SerializeField]
    private AudioSource hyperSpaceStart;
    [SerializeField]
    private AudioSource hyperSpaceEnd;

    public AudioSource HyperSpaceStart { get { return hyperSpaceStart; } }
    public AudioSource HyperSpaceEnd { get { return hyperSpaceEnd; } }

    private UI ui;

    private float lifeSupportTimer = -1f;

    private bool readyToWarp = false;

    void Start()
    {
        shipParts.AddRange(GetComponentsInChildren<ShipPart>());
        availableParts.AddRange(shipParts);
        health = healthPerModule;
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();

        UpdateReadyToWarp();
        UpdateLifeSupportStatus();
    }

    void Update()
    {
        if (lifeSupportTimer > 0)
        {
            ui.UpdateLifeSupportTimer(Mathf.Max(0.0f, lifeSupportTimer - Time.time) / lifeSupportTime);

            if (lifeSupportTimer < Time.time)
            {
                ui.YouDied();
            }
        }
    }
    public ShipPart LaunchRandomPart()
    {
        if (availableParts.Count <= 0)
        {
            return null;
        }
        var part = GetRandomPart();
        var dir = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        if (dir.magnitude < 0.01f)
        {
            dir = Vector2.down;
        }
        var force = dir.normalized * Random.Range(100f, 200f);
        part.Launch(force, shipParts);
        availableParts.Remove(part);

        UpdateLifeSupportStatus();
        UpdateReadyToWarp();
        return part;
    }

    public ShipPart LaunchRandomPartToTarget(Vector3 target)
    {
        if (availableParts.Count <= 0)
        {
            return null;
        }
        ShipPart part = GetRandomPart();
        Vector3 dir = target - part.transform.position;
        if (dir.magnitude < 0.01f)
        {
            dir = Vector2.down;
        }

        Vector3 force = dir.normalized * 6.5f * dir.magnitude;
        part.Launch(force, shipParts, target);
        availableParts.Remove(part);

        UpdateLifeSupportStatus();
        UpdateReadyToWarp();
        return part;
    }

    private ShipPart GetRandomPart()
    {
        int idx = Random.Range(0, availableParts.Count);
        return availableParts[idx];
    }

    public void AttachPart(ShipPart part)
    {
        if (!availableParts.Contains(part))
        {
            availableParts.Add(part);
        }

        UpdateLifeSupportStatus();
        UpdateReadyToWarp();
    }

    public void Wobble()
    {
        foreach (ShipPart part in availableParts)
        {
            if (part.IsDocked)
            {
                part.Wobble();
            }
        }
    }

    public void Hurt(float damage)
    {

        ShipPart launchedPart = null;
        health -= damage;
        if (health <= 0)
        {
            launchedPart = LaunchRandomPart();
            health = healthPerModule;
        }
    }

    public int CountLifeSupports()
    {
        int result = 0;
        foreach (var part in availableParts)
        {
            if (part.isLifeSupport)
            {
                result++;
            }
        }
        return result;
    }

    public int CountEngines()
    {
        int result = 0;
        foreach (var part in availableParts)
        {
            if (!part.isLifeSupport)
            {
                result++;
            }
        }
        return result;
    }

    private void UpdateReadyToWarp()
    {
        readyToWarp = CountEngines() == 2;
        if (!readyToWarp)
        {
            ui.ShowWarpDamaged();
        }
        else
        {
            ui.HideWarpDamaged();
        }
    }

    public bool IsReadyToWarp()
    {
        return readyToWarp;
    }

    private void UpdateLifeSupportStatus()
    {
        int lifeSupports = CountLifeSupports();

        if (lifeSupports == 0)
        {
            ui.ShowLifeSupportWarning();
            ui.HideLifeSupportDamaged();
            if (lifeSupportTimer < 0.0f)
            {
                lifeSupportTimer = Time.time + lifeSupportTime;
            }
            ui.UpdateLifeSupportTimer(Mathf.Max(0.0f, lifeSupportTimer - Time.time) / lifeSupportTime);
        }
        else if (lifeSupports < 3)
        {
            lifeSupportTimer = -1.0f;
            ui.HideLifeSupportWarning();
            ui.ShowLifeSupportDamaged();
        }
        else
        {
            lifeSupportTimer = -1.0f;
            ui.HideLifeSupportWarning();
            ui.HideLifeSupportDamaged();
        }
    }
}
