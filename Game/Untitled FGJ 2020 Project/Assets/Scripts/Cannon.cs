﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    [Range(-90, 90)]
    private float direction;
    [SerializeField]
    private GameObject barrel;
    [SerializeField]
    private ParticleSystem chargingParticleSystem;
    ParticleSystem.EmissionModule chargingEmission;
    [SerializeField]
    private float chargingTime = 5f;
    private float chargeStarted = 0f;
    [SerializeField]
    private GameObject laser;
    private float laserScaleMultiplier = 15f;
    private float laserShootSpeed = 375f;
    private float laserHitTime = 1f;
    private float laserHitStarted = -1f;
    [SerializeField]
    private ParticleSystem laserHitParticleSystem;
    [SerializeField]
    private float cooldownTime = 5f;
    private float cooldownStarted = -1f;
    
    private MotherShip motherShip;
    Vector3 startPos;
    private CannonStates state = CannonStates.Cooling;
    
    private float damageMin = 6.0f;
    private float damageMax = 6.0f;

    void Start()
    {
        barrel.transform.localEulerAngles = Vector3.zero;
        startPos = barrel.transform.up;
        chargingParticleSystem.Stop();
        laserHitParticleSystem.Stop();
        ParticleSystem.MainModule main = chargingParticleSystem.main;
        main.duration = chargingTime;
        chargeStarted = Time.time;
        chargingEmission = chargingParticleSystem.emission;
        motherShip = GameObject.FindGameObjectWithTag("MotherShip").GetComponent<MotherShip>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == CannonStates.Charging)
        {
            if (!chargingParticleSystem.isPlaying)
            {
                chargingEmission.enabled = true;
                chargingParticleSystem.Play();
                chargeStarted = Time.time;
            }

            if(chargeStarted + chargingTime < Time.time)
            {
                chargingEmission.enabled = false;
                chargingParticleSystem.Stop();
                state = CannonStates.Aiming;
            }
        }
        else
        {
            if (chargingParticleSystem.isPlaying)
            {
                chargingEmission.enabled = false;
                chargingParticleSystem.Stop();
            }
        }

        if (state == CannonStates.Aiming || state == CannonStates.Shooting)
        {
            Vector3 shipPos = motherShip.transform.position;
            Vector3 curPos = barrel.transform.position;
            var shipDir = shipPos - curPos;

            //TODO: limit angle to 180 degrees
            if (state == CannonStates.Aiming)
            {
                float angleDiff = Vector3.SignedAngle(barrel.transform.up, shipDir, Vector3.forward);
                float rotateDir = angleDiff < 0 ? -1 : 1;
                float rotateAmount = Mathf.Min(Mathf.Abs(angleDiff), Time.deltaTime * 90);
                barrel.transform.Rotate(Vector3.forward, rotateAmount * rotateDir);

                if (rotateAmount < 0.1f)
                {
                    state = CannonStates.Shooting;
                }
            }

            if (state == CannonStates.Shooting)
            {
                float targetDistance = Vector3.Distance(transform.position, motherShip.transform.position);
                Vector3 scale = laser.transform.localScale;

                Vector3 target = barrel.transform.position;
                Vector3 objectPos = laserHitParticleSystem.transform.position;
                target.x = target.x - objectPos.x;
                target.y = target.y - objectPos.y;

                float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg + 90;
                laserHitParticleSystem.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                laserHitParticleSystem.transform.Rotate(0, 180, 0);


                if (targetDistance > (scale.x / laserScaleMultiplier))
                {
                    laser.transform.localScale = new Vector3(scale.x + laserShootSpeed*Time.deltaTime, 1, 1);
                }
                else
                {
                    barrel.transform.Rotate(Vector3.forward, 5 * Time.deltaTime);
                    laserHitParticleSystem.transform.position = laser.transform.position + laser.transform.right * scale.x / laserScaleMultiplier * 0.89f;
                    if (laserHitStarted < 0)
                    {
                        laserHitStarted = Time.time;
                        laserHitParticleSystem.Play();
                        laserHitParticleSystem.transform.position = laser.transform.position + laser.transform.right * scale.x / laserScaleMultiplier;
                        motherShip.Wobble();
                    }
                    if(laserHitStarted + laserHitTime < Time.time)
                    {
                        state = CannonStates.Cooling;
                        laser.transform.localScale = new Vector3(1, 1, 1);
                        laserHitParticleSystem.Stop();
                        laserHitStarted = -1f;
                        HurtShip();
                    }
                }
            }
        }

        if (state == CannonStates.Cooling)
        {
            if (cooldownStarted < 0)
            {
                cooldownStarted = Time.time;
            }

            float angleDiff = Vector3.SignedAngle(barrel.transform.up, startPos, Vector3.forward);
            float rotateDir = angleDiff < 0 ? -1 : 1;
            float rotateAmount = Mathf.Min(Mathf.Abs(angleDiff), Time.deltaTime * 90);
            barrel.transform.Rotate(Vector3.forward, rotateAmount * rotateDir);

            if(cooldownStarted + cooldownTime < Time.time)
            {
                state = CannonStates.Charging;
                cooldownStarted = -1f;
            }
        }
    }

    private void HurtShip() {
        motherShip.Hurt(Random.Range(damageMin, damageMax));
    }
}

public enum CannonStates
{
    Charging,
    Aiming,
    Shooting,
    Cooling
}