using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 5;
    public float timeBetweenAttacks = 2f;

    private float timer;
    private bool isPlayerInContact;

    private Animator anim;
    private GameObject player;
    private PlayerHealth playerHealth;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    
    void FixedUpdate()
    {
        // 
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
            timer = Mathf.Clamp(timer, 0f, timeBetweenAttacks);
        }
        
        if (isPlayerInContact && timer <= 0f)
        {
            timer = timeBetweenAttacks;
            anim.SetBool("IsWalking", false);
            anim.SetBool("ReadyToAttack", true);
            anim.SetInteger("RandomAttackIndex", new Random().Next(1, 4));
            playerHealth.TakeDamage(damage);
        }
        
        if (timer <= timeBetweenAttacks - 0.5f && !IsAttacking())
        {
            anim.SetInteger("RandomAttackIndex", 0);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.Equals(player))
        {
            isPlayerInContact = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.Equals(player))
        {
            isPlayerInContact = false;
        }
    }

    private bool IsAttacking()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("attack_01")
               || anim.GetCurrentAnimatorStateInfo(0).IsName("attack_02")
               || anim.GetCurrentAnimatorStateInfo(0).IsName("attack_03");
    }

}
