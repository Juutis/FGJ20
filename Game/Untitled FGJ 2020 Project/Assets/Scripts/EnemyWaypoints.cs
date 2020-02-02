using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyWaypoints : MonoBehaviour
{
    private List<Transform> waypoints;
    private EnemyMovement enemy;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = new List<Transform>();
        Transform waypointParent = null;
        foreach(Transform child in transform)
        {
            if(child.tag == "Waypoints")
            {
                waypointParent = child;
            }
            else if(child.tag == "Enemy")
            {
                EnemyMovement e = child.GetComponent<EnemyMovement>();
                if (e != null)
                {
                    enemy = e;
                }
            }
        }

        foreach(Transform child in waypointParent)
        {
            waypoints.Add(child);
        }

        List<Vector3> positions = waypoints.Select(x => x.position).ToList();
        enemy.SetWaypoints(positions);
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Count <= 1) return;

        for(int i = 1; i < waypoints.Count; i++)
        {
            Debug.DrawLine(waypoints[i-1].position, waypoints[i].position, Color.cyan);
        }

        Debug.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position, Color.cyan);

        /*List<Vector3> positions = waypoints.Select(x => x.position).ToList();*/
    }
}
