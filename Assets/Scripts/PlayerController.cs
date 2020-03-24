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
    private Camera cam;
    private Attack attackScript;

    void Start()
    {
        cam = Camera.main;
        playerRigidbody = GetComponent<Rigidbody>();
        attackScript = GetComponentInChildren<Attack>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        playerRigidbody.velocity = playerInput * finalMovementSpeed;
        Rotate();
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

        if (other.tag == "EnemyWeapon")
        {
            attackScript.GetHit(other.GetComponentInParent<EnemyController>().Damage);
        }
    }
    
    private void StatisticsChangeByPickup() //Changes statistics of the player
    {
        if (timeStamp > Time.time)
        {
            finalMovementSpeed = basicMovementSpeed * pickup.GetComponent<PickupStatistics>().speedModifier;
            attackScript.damageModifier = pickup.GetComponent<PickupStatistics>().damageModifier;
            attackScript.ChangeHealthValue(pickup.GetComponent<PickupStatistics>().healthBonus);
            attackScript.enemyDamageModifier = pickup.GetComponent<PickupStatistics>().enemyDamageModifier;
        }          
        
        if (timeStamp < Time.time)
        {
            finalMovementSpeed = basicMovementSpeed;
            attackScript.damageModifier = 1.0f;
            attackScript.enemyDamageModifier = 1.0f;
            attackScript.wasHealthModified = false;
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

    private void Rotate()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5;
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);

        float rotSpeed = 360f;

        // distance between target and the actual rotating object
        Vector3 D = target - transform.position;


        // calculate the Quaternion for the rotation
        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(D), rotSpeed * Time.deltaTime);

        //Apply the rotation 
        transform.rotation = rot;

        // put 0 on the axys you do not want for the rotation object to rotate
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
