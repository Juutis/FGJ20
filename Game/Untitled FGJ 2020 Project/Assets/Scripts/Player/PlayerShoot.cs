using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    private Transform leftCannon;
    private Transform rightCannon;

    private Rigidbody2D rb2d;
    private float intervalTimer = 0f;

    public ShootingConfig config;

    void Start()
    {
        FindCannons();
        config = (ShootingConfig) Resources.Load("Configs/ShootingConfig");
        if (config == null) {
            Debug.LogWarning("Couldn't find Resources/Configs/ShootingConfig!");
        }
        if (leftCannon == null || rightCannon == null) {
            Debug.LogWarning("Couldn't find cannons!");
        }
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FindCannons() {
        leftCannon = FindChildObject("leftCannon");
        rightCannon = FindChildObject("rightCannon");
    }

    private Transform FindChildObject (string name) {
        foreach(Transform child in transform) {
            if (child.name == name) {
                return child.gameObject.transform;
            }
        }
        return null;
    }

    void Update()
    {
        intervalTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftControl) && intervalTimer > config.MinInterval) {
            Shoot();
            intervalTimer = 0f;
        }
    }

    void Shoot() {
        if (ProjectileManager.main == null) {
            Debug.LogWarning("Your scene does not have a ProjectileManager!");
        }
        ProjectileManager.main.SpawnProjectile(new ProjectileOptions() {
            LifeTime = config.LifeTime,
            Speed = config.Speed,
            Direction = leftCannon.up,
            StartingVelocity = rb2d.velocity,
            Position = leftCannon.position,
            Rotation = leftCannon.rotation
        });
        ProjectileManager.main.SpawnProjectile(new ProjectileOptions() {
            LifeTime = config.LifeTime,
            Speed = config.Speed,
            Direction = rightCannon.up,
            StartingVelocity = rb2d.velocity,
            Position = rightCannon.position,
            Rotation = rightCannon.rotation
        });
    }
}
