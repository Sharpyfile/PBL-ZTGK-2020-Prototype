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
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //rigidbody.velocity = speed * 
    }
}
