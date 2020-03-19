using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    [SerializeField]
    private float maxHp;
    private float currentHp;

    [SerializeField]
    private float damage;

    private Animator anim;

    [SerializeField]
    private Slider hpSlider;

    private bool canAttackAgain = true;
    private bool attackPressed = false;
    private bool strongAttackEnabled = false;

    private float attackPressedTime;
    [SerializeField]
    private float timeForStrongAttack = 2.0f;

    [SerializeField]
    private MeshRenderer weaponMeshRen;

    public float Damage { get => damage; }

    public float CurrentHp
    {
        get
        {
            return currentHp;
        }

        set
        {
            currentHp = value;
            hpSlider.value = currentHp / maxHp;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        CurrentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        AttackInput();
    }

    void GetHit(float dmg)
    {
        CurrentHp -= dmg;

        if (CurrentHp < 0)
        {
            CurrentHp = 0;
            Debug.Log("Dead");
        }
    }

    void AttackInput()
    {
        if (Input.GetButtonDown("Attack") && canAttackAgain)
        {
            attackPressed = true;
            attackPressedTime = Time.time;
        }

        if (attackPressed && Time.time - attackPressedTime >= timeForStrongAttack)
        {
            weaponMeshRen.material.color = Color.red;
            attackPressed = false;
        }

        if (Input.GetButtonUp("Attack"))
        {
            anim.SetTrigger("attack");
            canAttackAgain = false;
            attackPressed = false;
            weaponMeshRen.material.color = Color.white;
        }
    }

    public void EnableAttackingAgain()
    {
        canAttackAgain = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyWeapon")
        {
            GetHit(other.GetComponentInParent<EnemyController>().Damage);
        }
    }
}
