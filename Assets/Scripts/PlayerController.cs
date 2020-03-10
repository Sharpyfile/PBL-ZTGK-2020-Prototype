using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    private Rigidbody playerRigidbody;
    public ForceMode forceMode = ForceMode.Acceleration;
    private Vector3 playerInput;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        playerRigidbody.velocity = playerInput * movementSpeed;
    }
}
