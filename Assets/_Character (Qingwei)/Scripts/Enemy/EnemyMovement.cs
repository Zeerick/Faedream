using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class EnemyMovement : MonoBehaviour
{
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 2.5f;
    public float spottingRange = 15f;    // Range, with in which the enemy will spot the player and start chasing.
    public float walkingAndRunningBoundary = 5f;    // Critical distance between this enemy and the player, within which the enemy walks, otherwise runs.
    public float tauntingProbability = 0.5f;    // Represents probability that each enemy taunts (when states change). Valid values between 0 and 1.
    
    private bool isAlerted;
    private bool isPlayerInContact = false;
    private float playerDistance;
    
    private NavMeshAgent nav;
    private Animator anim;
    private GameObject player;
    private EnemyHealth enemyHealth;
    private PlayerHealth playerHealth;

    public bool IsAlerted
    {
        get => isAlerted;
        set => isAlerted = value;
    }

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.enabled = isAlerted;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyHealth = GetComponent<EnemyHealth>();
        playerHealth = player.GetComponent<PlayerHealth>();
        
        anim.SetInteger("RandomIdleIndex", new Random().Next(0, 2));
    }
    
    void FixedUpdate()
    {
        playerDistance = Vector3.Distance(transform.position, player.transform.position);    // Calculates by frame the distance between the player and this enemy.

        isAlerted = playerDistance <= spottingRange;    // If player is in the spotting range, set the enemy alerted. Vice versa.

        if (anim.GetBool("IsAlerted") != isAlerted)
        {
            TauntByChance();
        }
        anim.SetBool("IsAlerted", isAlerted);    // Pass the alerted information to animator.
      
        if (playerDistance > walkingAndRunningBoundary)
        {
            nav.speed = runningSpeed;
            anim.SetBool("IsRunning", true);
            TauntByChance();
        }
        else
        {
            nav.speed = walkingSpeed;
            anim.SetBool("IsRunning", false);
            TauntByChance();
        }
        
        if (!anim.GetBool("IsWalking"))
        {
            anim.SetBool("IsRunning", false);
        }
        
        if (isAlerted && enemyHealth.CurrentHealth > 0 && playerHealth.CurrentHealth > 0)
        {
            if (isPlayerInContact)
            {
                StopMoving();
            }
            else if (IsMobile())
            {
                nav.enabled = true;
                anim.SetBool("IsWalking", true);
                nav.SetDestination(player.transform.position);
            }
            else
            {
                StopMoving();
            }
            
        }
        else
        {
            StopMoving();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.Equals(player))
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

    private bool IsMobile()
    {
        return !(anim.GetCurrentAnimatorStateInfo(0).IsName("default")
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("idle_02")
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("idle_03") 
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("attack_01")
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("attack_02")
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("attack_03")
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("defend")
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("taunt")
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("taunt 1")
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("getHit")
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("die") );
    }
    
    private void StopMoving()
    {
        nav.enabled = false;
        anim.SetBool("IsWalking", false);
        anim.SetBool("IsRunning", false);
    }

    private void TauntByChance()
    {
        if (new Random().NextDouble() < tauntingProbability)
        {
            anim.SetTrigger("Taunt");
        }
    }
    
}
