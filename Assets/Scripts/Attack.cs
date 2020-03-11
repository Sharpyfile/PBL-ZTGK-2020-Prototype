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
            anim.SetTrigger("attack");
            canAttackAgain = false;
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
