
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "PlayerMovementConfig", menuName = "New PlayerMovementConfig")]
public class PlayerMovementConfig : ScriptableObject
{


    [SerializeField]
    [Range(0, 50f)]
    private float mass = 1f;
    public float Mass { get { return mass; } }


    [SerializeField]
    [Range(0, 50f)]
    private float angularDrag = 5f;
    public float AngularDrag { get { return angularDrag; } }

    [SerializeField]
    [Range(0, 50f)]
    private float linearDrag = 5f;
    public float LinearDrag { get { return linearDrag; } }
    
    [SerializeField]
    [Range(0, 50f)]
    private float rotationSpeed = 5f;
    public float RotationSpeed { get { return rotationSpeed; } }


    [SerializeField]
    private ForceMode2D rotationForceMode2D;
    public ForceMode2D RotationForceMode2D {get {return rotationForceMode2D;}}

    [SerializeField]
    [Range(0, 50f)]
    private float velocityMagnitudeMax = 5f;
    public float VelocityMagnitudeMax { get { return velocityMagnitudeMax; } }

    [SerializeField]
    [Range(0, 50f)]
    private float forwardSpeed = 5f;
    public float ForwardSpeed { get { return forwardSpeed; } }

    [SerializeField]
    private ForceMode2D speedForceMode2D;
    public ForceMode2D SpeedForceMode2D {get {return speedForceMode2D;}}

    [SerializeField]
    [Range(0, 50f)]
    private float backwardSpeed = 2.5f;
    public float BackwardSpeed { get { return backwardSpeed; } }


}
