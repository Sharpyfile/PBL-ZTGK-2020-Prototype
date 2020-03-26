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
    [SerializeField]
    private float comboDamage;

    private float currentDamage;

    [HideInInspector]
    public float damageModifier = 1.0f;
    [HideInInspector]
    public float enemyDamageModifier = 1.0f;
    [HideInInspector]
    public bool wasHealthModified = false;

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

    public enum AttackType
    {
        NONE,
        LEFT_WEAK,
        LEFT_STRONG,
        RIGHT_WEAK,
        RIGHT_STRONG,
        BOTH_STRONG
    }

    private AttackType[] recentAttacks;
    private AttackType lastAttack = AttackType.NONE;

    private float attackTimeStamp = 0f;

    public float AttackTimeStamp { get => attackTimeStamp; }

    [SerializeField]
    private float timeForCombo = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        CurrentHp = maxHp;
        currentDamage = damage * damageModifier;
        recentAttacks = new AttackType[3];
        recentAttacks[0] = AttackType.NONE;
        recentAttacks[1] = AttackType.NONE;
        recentAttacks[2] = AttackType.NONE;
        StartCoroutine(ClearRecentAttacks());
    }

    // Update is called once per frame
    void Update()
    {
        AttackInputR();
        AttackInputL();
        CheckForCombo();
    }

    public void GetHit(float dmg)
    {
        Debug.Log(dmg * enemyDamageModifier);
        CurrentHp -= dmg * enemyDamageModifier;
        
        if (CurrentHp < 0)
        {
            CurrentHp = 0;
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
                AttackNow(AttackType.BOTH_STRONG);
                canAttackAgainR = false;
                attackPressedR = false;
                weaponMeshRenR.material.color = Color.white;
                canAttackAgainL = false;
                attackPressedL = false;
                weaponMeshRenL.material.color = Color.white;
            }
            else
            {
                if (attackLoadedR)
                {
                    currentDamage = strongDamage;
                    AttackNow(AttackType.RIGHT_STRONG);
                }
                else
                {
                    currentDamage = damage;
                    AttackNow(AttackType.RIGHT_WEAK);
                }

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
                if (attackLoadedL)
                {
                    currentDamage = strongDamage;
                    AttackNow(AttackType.LEFT_STRONG);
                }
                else
                {
                    currentDamage = damage;
                    AttackNow(AttackType.LEFT_WEAK);
                }

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

    private void PushRecentAttack( AttackType type )
    {
        recentAttacks[0] = recentAttacks[1];
        recentAttacks[1] = recentAttacks[2];
        recentAttacks[2] = type;

        Debug.Log("Recents: " + recentAttacks[0].ToString() + " | "
            + recentAttacks[1].ToString() + " | "
            + recentAttacks[2].ToString());
    }

    private IEnumerator ClearRecentAttacks()
    {
        while(true)
        {
            if (recentAttacks[0] != AttackType.NONE)
            {
                recentAttacks[0] = AttackType.NONE;
            }
            else if (recentAttacks[1] != AttackType.NONE)
            {
                recentAttacks[1] = AttackType.NONE;
            }
            else if (recentAttacks[2] != AttackType.NONE)
            {
                recentAttacks[2] = AttackType.NONE;
            }

            yield return new WaitForSecondsRealtime(timeForCombo);
        }
    }

    private void CheckForCombo()
    {
        if ((recentAttacks[0] == AttackType.RIGHT_STRONG || recentAttacks[0] == AttackType.LEFT_STRONG)
            && ((recentAttacks[1] == AttackType.RIGHT_WEAK && recentAttacks[2] == AttackType.LEFT_WEAK) 
            || (recentAttacks[1] == AttackType.LEFT_WEAK && recentAttacks[2] == AttackType.RIGHT_WEAK)))
        {
            currentDamage = comboDamage;
            anim.ResetTrigger("attackL");
            anim.ResetTrigger("attackR");
            anim.SetTrigger("twist");
            AttackNow(AttackType.NONE);
            recentAttacks[0] = AttackType.NONE;
            recentAttacks[1] = AttackType.NONE;
            recentAttacks[2] = AttackType.NONE;
        }
    }

    private void AttackNow(AttackType type)
    {
        currentDamage *= damageModifier;
        Debug.Log("Current damage: " + currentDamage.ToString());
        lastAttack = type;
        attackTimeStamp = Time.time;
    }

    public void AttackConnected()
    {
        PushRecentAttack(lastAttack);
    }

    public void ChangeHealthValue(float value)
    {
        if (!wasHealthModified)
        {
            CurrentHp += value;
            wasHealthModified = true;
        }
    }
}
