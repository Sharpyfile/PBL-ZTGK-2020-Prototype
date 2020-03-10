using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    private Rigidbody rigidbody;
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float velocityX;
    [SerializeField]
    private float velocityY;
    [SerializeField]
    private float fieldOfView;
    [SerializeField]
    private float neighbourDistance;
    private GameObject[] otherEnemies;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        otherEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        //rigidbody.velocity = speed * 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            if (other.gameObject.GetComponentInParent<Character>())
            {
                GetHit(other.gameObject.GetComponentInParent<Character>().Damage);
            }
        }
    }

    void GetHit(float dmg)
    {
        base.GetHit(dmg);
        CheckIfAlive();
    }

    void CheckIfAlive()
    {
        if (Health <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
