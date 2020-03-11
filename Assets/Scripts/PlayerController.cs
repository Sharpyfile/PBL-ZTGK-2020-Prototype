using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float basicMovementSpeed;
    public float finalMovementSpeed;
    private Rigidbody playerRigidbody;
    public ForceMode forceMode = ForceMode.Acceleration;
    private Vector3 playerInput;
    public GameObject pickup;
    private float timeStamp;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        playerRigidbody.velocity = playerInput * finalMovementSpeed;

        if (pickup != null)
        {
            HasPickup(); //Contains all functions that are done when player has pickup
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup") && pickup == null)
        {
            Debug.Log("Pickup detected");
            pickup = other.gameObject;
            timeStamp = Time.time + pickup.GetComponent<PickupStatistics>().durationTime;
            Debug.Log(timeStamp);
        }            
    }
    
    private void StatisticsChangeByPickup() //Changes statistics of the player
    {
        if (timeStamp > Time.time)
        {
            finalMovementSpeed = basicMovementSpeed * pickup.GetComponent<PickupStatistics>().speedModifier;
            Debug.Log("Pickup affecting speed" + (timeStamp - Time.time).ToString());
        }          
        
        if (timeStamp < Time.time)
        {
            finalMovementSpeed = basicMovementSpeed;
            Destroy(pickup.gameObject);
            pickup = null;
        }         
    }

    private void HasPickup() //Contains all
    {
        pickup.transform.position = playerRigidbody.transform.position;
        pickup.transform.position = pickup.transform.position + new Vector3(0, 1, 0);
        StatisticsChangeByPickup();
    }

}
