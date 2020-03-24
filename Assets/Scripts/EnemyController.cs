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

    private MeshRenderer ren;

    private Animator anim;

    private float attackTimeStamp = 0f;

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
            Attack attackScript = other.gameObject.GetComponentInParent<Attack>();
            if (attackScript && attackScript.AttackTimeStamp != attackTimeStamp)
            {
                GetHit(attackScript.Damage);
                attackScript.AttackConnected();
                Debug.Log("DAMAGE " + attackScript.Damage);
                attackTimeStamp = attackScript.AttackTimeStamp;
                Vector3 forceDir = (transform.position - other.transform.position).normalized;
                forceDir.y = 0f;
                rigidbody.AddForce(forceDir * 10f, ForceMode.Impulse);
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

    public void TurnEnemyRed()
    {
        ren.material.color = Color.red;
    }

    public void TurnEnemyWhite()
    {
        ren.material.color = Color.green;
    }
}
