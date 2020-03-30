using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    private Rigidbody rigidbody;
    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private float fieldOfView = 90.0f;
    [SerializeField]
    private float neighbourDistance = 10.0f;
    private GameObject[] otherEnemies;
    [SerializeField]
    private float wage = 0.01f;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float rotationSpeed = 3f;
    [SerializeField]
    private float MinimalDistance = 8.0f;

    private float sum = 0;
    private float avDist;
    private List<GameObject> neighbours;
    private bool leader;
    // Start is called before the first frame update

    private MeshRenderer ren;

    private Animator anim;

    private static float staticAttackTimeStamp = 0f;
    private float attackTimeStamp = 0f;

    [SerializeField]
    private AudioClip[] hitSounds;

    private AudioSource src;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        ren = GetComponentInChildren<MeshRenderer>();
        anim = GetComponent<Animator>();
        neighbours = new List<GameObject>();
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        sum = 0;
        neighbours.Clear();
        otherEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in otherEnemies)
        {
            if (IsInNeighbourhood(e) && e != this.gameObject)
            {
                neighbours.Add(e);
                sum += Distance(e);
            }
        }
        avDist = sum / neighbours.Count;

        Debug.Log(neighbours.Count);

        if(!leader)
        {
            Debug.Log(name + " I'm not a leader");
            BeInTheMiddle();
            PersonalSpace();
        }
        else
        {
            Debug.Log(name + "I'M A LEADER!!!");
        }

        Move();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            Attack attackScript = other.gameObject.GetComponentInParent<Attack>();
            if (attackScript)
            {
                if (attackScript.AttackTimeStamp != attackTimeStamp)
                {
                    GetHit(attackScript.Damage);
                    attackTimeStamp = attackScript.AttackTimeStamp;
                }

                if (attackScript.AttackTimeStamp != staticAttackTimeStamp)
                {
                    attackScript.AttackConnected();
                    staticAttackTimeStamp = attackScript.AttackTimeStamp;
                }
            }
        }
        else if (other.gameObject.tag == "ComboWeapon")
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
                rigidbody.AddForce(forceDir * 20f, ForceMode.Impulse);
            }
        }
    }

    void GetHit(float dmg)
    {
        src.clip = hitSounds[Random.Range(0, hitSounds.Length)];
        src.Play();
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

    bool IsInNeighbourhood(GameObject enemy)
    {
        float k1, k2;
        if (Distance(enemy) < neighbourDistance)
        {
            if (rigidbody.velocity.x != 0)
            {
                k1 = Mathf.Atan(rigidbody.velocity.z / rigidbody.velocity.x);
            }
            else
            {
                k1 = Mathf.Atan(rigidbody.velocity.z);
            }
            if (enemy.GetComponent<Rigidbody>().velocity.x - rigidbody.velocity.x != 0)
            {
                k2 = Mathf.Atan((enemy.GetComponent<Rigidbody>().velocity.z - rigidbody.velocity.z) /
                                      (enemy.GetComponent<Rigidbody>().velocity.x - rigidbody.velocity.x));
            }
            else if (Vector3.Dot(enemy.transform.position - transform.position, enemy.transform.forward) == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
            if (Mathf.Abs(k1 - k2) < fieldOfView)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private float Distance(GameObject enemy)
    {
        return Mathf.Sqrt(((this.transform.position.x - enemy.transform.position.x) * (this.transform.position.x - enemy.transform.position.x)) +
            ((this.transform.position.z - enemy.transform.position.z) * (this.transform.position.z - enemy.transform.position.z)));
    }

    private void Move()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                               Quaternion.LookRotation(player.position - transform.position),
                                               rotationSpeed * Time.deltaTime);

        rigidbody.velocity += transform.forward * speed * Time.deltaTime;
    }

    private void BeInTheMiddle()
    {
        float dist, newVelocityX, newVelocityZ;
        foreach (var n in neighbours)
        {
            dist = Distance(n);
            newVelocityX = rigidbody.velocity.x + wage * (((n.transform.position.x - transform.position.x) * (dist - avDist)) / dist);
            newVelocityZ = rigidbody.velocity.z + wage * (((n.transform.position.z - transform.position.z) * (dist - avDist)) / dist);
            Vector3 newVelocity = new Vector3(newVelocityX, 0, newVelocityZ);
            rigidbody.velocity = newVelocity;
        }
    }

    private void PersonalSpace()
    {
        float dist, newVelocityX, newVelocityZ;
        foreach (var n in neighbours)
        {
            dist = Distance(n);
            newVelocityX = rigidbody.velocity.x - wage * ((((n.transform.position.x - transform.position.x) * MinimalDistance) / dist) - (n.transform.position.x - transform.position.x));
            newVelocityZ = rigidbody.velocity.z - wage * ((((n.transform.position.z - transform.position.z) * MinimalDistance) / dist) - (n.transform.position.z - transform.position.z));
            Vector3 newVelocity = new Vector3(newVelocityX, 0, newVelocityZ);
            rigidbody.velocity = newVelocity;
        }
    }

    public void SetLeader()
    {
        foreach(var e in neighbours)
        {
            e.GetComponent<EnemyController>().UnSetLeader();
        }
        leader = true;
    }

    public void UnSetLeader()
    {
        leader = false;
    }
}
