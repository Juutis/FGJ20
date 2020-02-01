
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "PlayerMovementConfig", menuName = "New PlayerMovementConfig")]
public class PlayerMovementConfig : ScriptableObject
{
    [SerializeField]
    [Range(1f, 20f)]
    private float rotationSpeed = 5f;
    public float RotationSpeed { get { return rotationSpeed; } }

    [SerializeField]
    [Range(1f, 20f)]
    private float velocityMagnitudeMax = 5f;
    public float VelocityMagnitudeMax { get { return velocityMagnitudeMax; } }

    [SerializeField]
    [Range(1f, 20f)]
    private float forwardSpeed = 5f;
    public float ForwardSpeed { get { return forwardSpeed; } }

    [SerializeField]
    [Range(1f, 20f)]
    private float backwardSpeed = 2.5f;
    public float BackwardSpeed { get { return backwardSpeed; } }

    [SerializeField]
    private float inertia = 0.8f;
    public float Inertia { get { return inertia; } }

}
