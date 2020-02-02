using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableEnemy : MonoBehaviour
{
    [SerializeField]
    private int maxHP = 5;
    private int currentHP = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            Destroy(gameObject);
            /*destroyedParts.transform.parent = transform.parent;
            destroyedParts.SetActive(true);
            Vector3 center = transform.position;
            foreach (Transform child in destroyedParts.transform)
            {
                Vector3 direction = child.position - center;
                child.GetComponent<Rigidbody2D>().AddForce(direction.normalized * 100);
            }*/
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "PlayerProjectile")
        {
            currentHP--;
        }
    }
}
