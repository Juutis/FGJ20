using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShip : MonoBehaviour
{
    List<ShipPart> shipParts = new List<ShipPart>(),
        availableParts = new List<ShipPart>();


    // Start is called before the first frame update
    void Start()
    {
        shipParts.AddRange(GetComponentsInChildren<ShipPart>());
        availableParts.AddRange(shipParts);
    }

    // Update is called once per frame
    void Update()
    {
        LaunchRandomPart();

        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (var part in shipParts)
            {
                part.Repair();
            }
        }
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
    }

    private ShipPart getRandomPart()
    {
        int idx = Random.Range(0, availableParts.Count);
        return availableParts[idx];
    }

    public void AttachPart(ShipPart part)
    {
        availableParts.Add(part);
    }
}
