
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "ShootingConfig", menuName = "New ShootingConfig")]
public class ShootingConfig : ScriptableObject
{    
    [SerializeField]
    [Range(1f, 20f)]
    private float lifeTime = 5f;
    public float LifeTime {get { return lifeTime ;}}
    [SerializeField]
    [Range(1f, 10f)]
    private float speed = 5f;
    public float Speed {get { return speed;}}

    [SerializeField]
    [Range(0f, 3f)]
    private float minInterval = 1f;
    public float MinInterval {get { return minInterval;} }

}
