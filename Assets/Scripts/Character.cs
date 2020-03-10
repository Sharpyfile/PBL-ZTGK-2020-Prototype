using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float health;
    public float MaxHealth { get => maxHealth; }
    public float Damage { get => damage; }
    public float Health { get => health; }
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void GetHit(float dmg)
    {
        health -= dmg;
    }
}
