﻿using System.Collections;
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

    private MeshRenderer ren;

    private Animator anim;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        ren = GetComponentInChildren<MeshRenderer>();
        anim = GetComponent<Animator>();
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
            if (other.gameObject.GetComponentInParent<Attack>())
            {
                GetHit(other.gameObject.GetComponentInParent<Attack>().Damage);
            }
        }
    }

    void GetHit(float dmg)
    {
        base.GetHit(dmg);
        Debug.Log("Ouch! health left " + Health);
        CheckIfAlive();
    }

    void CheckIfAlive()
    {
        if (Health <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public void TurnEnemyRed()
    {
        ren.sharedMaterial.color = Color.red;
    }

    public void TurnEnemyWhite()
    {
        ren.sharedMaterial.color = Color.green;
    }
}
