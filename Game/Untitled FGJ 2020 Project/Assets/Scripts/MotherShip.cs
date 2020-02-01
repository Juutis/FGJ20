using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShip : MonoBehaviour
{
    [SerializeField]
    float healthPerModule = 10f;

    [SerializeField]
    float lifeSupportTime = 60f;

    float health;

    List<ShipPart> shipParts = new List<ShipPart>(),
        availableParts = new List<ShipPart>();

    UI ui;

    private float lifeSupportTimer = -1f;

    bool readyToWarp = false;


    // Start is called before the first frame update
    void Start()
    {
        shipParts.AddRange(GetComponentsInChildren<ShipPart>());
        availableParts.AddRange(shipParts);
        health = healthPerModule;
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (var part in shipParts)
            {
                part.Repair();
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Hurt(5);
        }

        if (lifeSupportTimer > 0)
        {
            ui.UpdateLifeSupportTimer(Mathf.Max(0.0f, lifeSupportTimer - Time.time));
        }
        Debug.Log(countEngines() + " " + countLifeSupports());
    }

    public void LaunchRandomPart()
    {
        if (availableParts.Count <= 0)
        {
            return;
        }
        var part = getRandomPart();
        var dir = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        if (dir.magnitude < 0.01f)
        {
            dir = Vector2.down;
        }
        var force = dir.normalized * Random.Range(2000f, 3000f);
        part.Launch(force);
        availableParts.Remove(part);

        if (countLifeSupports() <= 0)
        {
            ui.ShowLifeSupportWarning();
            if (lifeSupportTimer < 0.0f)
            {
                lifeSupportTimer = Time.time + lifeSupportTime;
            }
            ui.UpdateLifeSupportTimer(Mathf.Max(0.0f, lifeSupportTimer - Time.time));
        }

        updateReadyToWarp();
    }

    private ShipPart getRandomPart()
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

        if (countLifeSupports() > 0)
        {
            ui.HideLifeSupportWarning();
            lifeSupportTimer = -1.0f;
        }

        updateReadyToWarp();
    }

    public void Hurt(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            LaunchRandomPart();
            health = healthPerModule;
        }
    }

    private int countLifeSupports()
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

    private int countEngines()
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

    private void updateReadyToWarp()
    {
        readyToWarp = countEngines() == 2;
    }

    public bool IsReadyToWarp()
    {
        return readyToWarp;
    }
}
