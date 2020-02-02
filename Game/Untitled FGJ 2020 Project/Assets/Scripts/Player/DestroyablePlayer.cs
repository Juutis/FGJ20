using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyablePlayer : MonoBehaviour
{
    [SerializeField]
    private int maxHP = 5;
    private int currentHP = 5;
    private PlayerMovement player;
    private MotherShip motherShip;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        player = gameObject.GetComponent<PlayerMovement>();
        motherShip = GameObject.FindGameObjectWithTag("MotherShip").GetComponent<MotherShip>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            /*destroyedParts.transform.parent = transform.parent;
            destroyedParts.SetActive(true);
            Vector3 center = transform.position;
            foreach (Transform child in destroyedParts.transform)
            {
                Vector3 direction = child.position - center;
                child.GetComponent<Rigidbody2D>().AddForce(direction.normalized * 100);
            }*/
            if(player != null)
            {
                player.transform.position = motherShip.transform.position;
                player.Disable();
                Invoke("Activate", 1f);
            }
        }
    }

    private void Activate()
    {
        Debug.Log("prööt");
        player.Activate();
        currentHP = maxHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "EnemyProjectile")
        {
            currentHP--;
        }
    }
}
