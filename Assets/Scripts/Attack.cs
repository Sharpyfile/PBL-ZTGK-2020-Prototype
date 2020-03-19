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
    [SerializeField]
    private float strongDamage;

    private float currentDamage;

    private Animator anim;

    [SerializeField]
    private Slider hpSlider;

    private bool canAttackAgainR = true;
    private bool canAttackAgainL = true;
    private bool attackPressedR = false;
    private bool attackPressedL = false;
    private bool attackLoadedR = false;
    private bool attackLoadedL = false;
    private bool cancelAttackL = false;

    private float attackPressedTimeR;
    private float attackPressedTimeL;
    [SerializeField]
    private float timeForStrongAttack = 2.0f;

    [SerializeField]
    private MeshRenderer weaponMeshRenR;
    [SerializeField]
    private MeshRenderer weaponMeshRenL;

    public float Damage { get => currentDamage; }

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
        currentDamage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        AttackInputR();
        AttackInputL();
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

    void AttackInputR()
    {
        if (Input.GetButtonDown("AttackR") && canAttackAgainR)
        {
            attackPressedR = true;
            attackPressedTimeR = Time.time;
        }

        if (attackPressedR && Time.time - attackPressedTimeR >= timeForStrongAttack)
        {
            weaponMeshRenR.material.color = Color.red;
            attackPressedR = false;
            attackLoadedR = true;
        }

        if (Input.GetButtonUp("AttackR"))
        {
            if (attackLoadedR && attackLoadedL)
            {
                attackLoadedL = false;
                attackLoadedR = false;
                cancelAttackL = true;

                currentDamage = strongDamage;
                anim.SetTrigger("attackBoth");
                canAttackAgainR = false;
                attackPressedR = false;
                weaponMeshRenR.material.color = Color.white;
                canAttackAgainL = false;
                attackPressedL = false;
                weaponMeshRenL.material.color = Color.white;
            }
            else
            {
                currentDamage = attackLoadedR ? strongDamage : damage;
                anim.SetTrigger("attackR");
                canAttackAgainR = false;
                attackPressedR = false;
                attackLoadedR = false;
                weaponMeshRenR.material.color = Color.white;
            }
        }
    }

    void AttackInputL()
    {
        if (Input.GetButtonDown("AttackL") && canAttackAgainL)
        {
            attackPressedL = true;
            attackPressedTimeL = Time.time;
        }

        if (attackPressedL && Time.time - attackPressedTimeL >= timeForStrongAttack)
        {
            weaponMeshRenL.material.color = Color.red;
            attackPressedL = false;
            attackLoadedL = true;
        }

        if (Input.GetButtonUp("AttackL"))
        {
            if (!cancelAttackL)
            {
                currentDamage = attackLoadedL ? strongDamage : damage;
                anim.SetTrigger("attackL");
                canAttackAgainL = false;
                attackPressedL = false;
                attackLoadedL = false;
                weaponMeshRenL.material.color = Color.white;
            }

            cancelAttackL = false;
        }
    }

    public void EnableAttackingAgainR()
    {
        canAttackAgainR = true;
    }

    public void EnableAttackingAgainL()
    {
        canAttackAgainL = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyWeapon")
        {
            GetHit(other.GetComponentInParent<EnemyController>().Damage);
        }
    }
}
