using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{

    private float lifeTime = 5f;

    private float lifeTimer = 0f;

    private Rigidbody2D rb2d;

    public void Sleep() {
        ProjectileManager.main.Sleep(this);
    }

    public void Deactivate() {
        rb2d.velocity = Vector3.zero;
        lifeTimer = 0f;
    }

    public void Initialize(ProjectileOptions options) {
        if (rb2d == null) {
            rb2d = GetComponent<Rigidbody2D>();
        }
        transform.position = options.Position;
        transform.rotation = options.Rotation;
        this.lifeTime = options.LifeTime;
        rb2d.velocity = options.StartingVelocity;
        rb2d.AddForce(options.Direction * options.Speed, ForceMode2D.Impulse);
        gameObject.tag = options.Tag;
        gameObject.layer = LayerMask.NameToLayer(options.Layer);
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeTime) {
            Sleep();
        }
    }

    public void Activate() {

    }

    private void OnTriggerEnter2D(Collider2D collider2d) {
        Sleep();
    }
}
