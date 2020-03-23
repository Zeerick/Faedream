using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public float timeBetweenDamages = 1f;
    
    private int currentHealth;
    private bool isDead = false;
    private float timer;

    private CapsuleCollider hitBox;
    private Animator anim;

    void Start()
    {
        hitBox = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy") && timer >= timeBetweenDamages)
        {
            // TakeDamage(other.GetComponent<EnemyDamage>().Damage);
        }
    }

    public int CurrentHealth => currentHealth;  // Important for integrating with UI that displays player health information.

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
            isDead = true;
        }
    }

    private void Die()
    {
        // tbi
    }

}
