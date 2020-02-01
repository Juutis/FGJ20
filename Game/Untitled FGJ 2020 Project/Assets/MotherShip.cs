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
        var force = new Vector2(Random.Range(-3000f, 3000f), Random.Range(-3000f, 3000f));
        part.Launch(force);
        availableParts.Remove(part);
    }

    private ShipPart getRandomPart()
    {
        int idx = Random.Range(0, availableParts.Count);
        return availableParts[idx];
    }
}
