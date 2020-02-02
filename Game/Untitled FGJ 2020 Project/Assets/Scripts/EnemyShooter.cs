using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private float shootDistance = 12f;
    private Transform leftCannon;
    private Transform rightCannon;

    private float intervalTimer = 0f;
    public ShootingConfig config;

    void Start()
    {
        FindCannons();
        config = (ShootingConfig)Resources.Load("Configs/EnemyShootingConfig");
        if (config == null)
        {
            Debug.LogWarning("Couldn't find Resources/Configs/EnemyShootingConfig!");
        }
        if (leftCannon == null || rightCannon == null)
        {
            Debug.LogWarning("Couldn't find cannons!");
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FindCannons()
    {
        leftCannon = FindChildObject("leftCannon");
        rightCannon = FindChildObject("rightCannon");
    }

    private Transform FindChildObject(string name)
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.name == name)
            {
                return child.gameObject.transform;
            }
        }
        return null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool playerVisible = false;
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int hitCount = Physics2D.LinecastNonAlloc(transform.position, player.transform.position, hits);

        if(hitCount >= 1)
        {
            RaycastHit2D hit = hits[0];
            if(hit.transform.tag == "Player")
            {
                playerVisible = true;
            }
        }

        if(Vector3.Distance(player.transform.position, transform.position) < shootDistance)
        {
            Debug.Log("shoot");
            float angleDiff = Vector3.SignedAngle(transform.up, player.transform.position - transform.position, Vector3.forward);
            float rotateDir = angleDiff < 0 ? -1 : angleDiff > 0 ? 1 : 0;
            /*ActivateLeftNavigationThruster(false);
            ActivateRightNavigationThruster(false);
            if (rotateDir > 0.01f) ActivateRightNavigationThruster(true);
            else if (rotateDir < -0.01f) ActivateLeftNavigationThruster(true);*/
            float rotateAmount = Mathf.Min(Mathf.Abs(angleDiff), Time.deltaTime * 90);
            transform.Rotate(Vector3.forward, rotateAmount * rotateDir);

            intervalTimer += Time.deltaTime;
            if (intervalTimer > config.MinInterval)
            {
                Shoot();
                intervalTimer = 0f;
            }
        }
    }

    void Shoot()
    {
        if (ProjectileManager.main == null)
        {
            Debug.LogWarning("Your scene does not have a ProjectileManager!");
        }

        Vector3 v3 = (player.transform.position - transform.position).normalized;
        Vector2 vec = new Vector2(v3.x, v3.y);

        ProjectileManager.main.SpawnProjectile(new ProjectileOptions()
        {
            LifeTime = config.LifeTime,
            Speed = config.Speed,
            Direction = leftCannon.up,
            StartingVelocity = vec,//rb2d.velocity,
            Position = leftCannon.position,
            Rotation = leftCannon.rotation,
            Tag = "EnemyProjectile",
            Layer = "EnemyProjectile"
        });
        ProjectileManager.main.SpawnProjectile(new ProjectileOptions()
        {
            LifeTime = config.LifeTime,
            Speed = config.Speed,
            Direction = rightCannon.up,
            StartingVelocity = Vector2.zero,//rb2d.velocity,
            Position = rightCannon.position,
            Rotation = rightCannon.rotation,
            Tag = "EnemyProjectile",
            Layer = "EnemyProjectile"
        });
    }
}
