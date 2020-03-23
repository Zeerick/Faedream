using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public enum Buff
    {
        burning,
        frozen,
        slowed
    }

    public int maxHealth = 100;
    public int shieldDefence = 5;

    private int currentHealth;
    private bool isDead = false;
    private HashSet<Buff> buffs;

    private Rigidbody rb;
    private CapsuleCollider cc;
    private Animator anim;
    private GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
    }
    
    void FixedUpdate()
    {
        if (isDead)
        {
            // to be implemented if needed
        }
    }
    
    public int CurrentHealth => currentHealth;  // Might be important for integrating with UI that displays player health information.

    public bool IsDead => isDead;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("GetHit");
        }
    }

    private void Die()
    {
        anim.SetTrigger("Die");
        isDead = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        // cc.isTrigger = true;
        cc.enabled = false;
    }
    
}
