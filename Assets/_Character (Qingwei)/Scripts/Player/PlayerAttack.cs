using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // public Vector3 castPosition = new Vector3(0, 0, 0);    // Might be useful for gameplay options, bit not applicable yet.

    public GameObject fireBall;
    public int fireBall_damage = FireBall.damage;
    public float fireBall_forwardSpeed = FireBall.forwardSpeed;
    public float fireBall_range = FireBall.range;
    public float fireBall_expansion = FireBall.expansion;
    public float fireBall_cooldown = 3f;
    private float fireBall_cooldownTimer = 0f;

    public GameObject snowBall;
    public int snowBall_damage = SnowBall.damage;
    public float snowBal_forwardSpeed = SnowBall.forwardSpeed;
    public float snowBall_cooldown = 3f;
    private float snowBall_cooldownTimer = 0f;
    
    public GameObject bouncingBall;
    public int bouncingball_damage = BouncingBall.damage;
    public bool bouncingBall_isRealistic = BouncingBall.isRealistic;
    public float bouncingBall_forwardSpeed = BouncingBall.forwardSpeed;
    public float bouncingBall_bouncingForce = BouncingBall.bouncingForce;
    public int bouncingBall_bouncingTime = BouncingBall.maxBouncingTime;
    public float bouncingBall_forwardForce = BouncingBall.forwardForce;
    public float bouncingBall_bouncingAttenuation = BouncingBall.bouncingAttenuation;
    public float bouncingBall_cooldown = 3f;
    private float bouncingBall_cooldownTimer = 0f;
    
    public GameObject explosion;
    private GameObject camera;

    private bool isCastingSorcery = false;
    
    private LayerMask shootableMask;
    private LineRenderer aimLazer;
    
    private Ray aimRay;
    private RaycastHit aimLazerHit;

    private ParabolaRay parabola;
    
    // public ParticleSystem.Particle explosion;
    
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");    // Automatically assign the variable to the main camera. This requires the main camera to have tag "MainCamera".
        shootableMask = LayerMask.GetMask("Shootable");
        aimLazer = GetComponent<LineRenderer>();
        parabola = GetComponent<ParabolaRay>();
    }

    private void Update()
    {
        // Displays fire ball aiming effect
        if (Input.GetKeyDown(KeyCode.Alpha1) && fireBall_cooldownTimer <= 0f)
        {
            isCastingSorcery = true;
            AimFireball();
            aimLazer.enabled = true;
        }
        if (isCastingSorcery && Input.GetKey(KeyCode.Alpha1) && fireBall_cooldownTimer <= 0f)
        {
            AimFireball();
            aimLazer.enabled = true;
        }
        
        // Displays snow ball aiming effect
        if (Input.GetKeyDown(KeyCode.Alpha2) && snowBall_cooldownTimer <= 0f)
        {
            isCastingSorcery = true;
            AimSnowball();
            parabola.enabled = true;
            aimLazer.enabled = true;
        }
        if (isCastingSorcery && Input.GetKey(KeyCode.Alpha2) && snowBall_cooldownTimer <= 0f)
        {
            AimSnowball();
            parabola.enabled = true;
            aimLazer.enabled = true;
        }

        // Allows cancelling sorceries when targeting them, by pressing right mouse button.
        if (isCastingSorcery && Input.GetMouseButtonDown(1))
        {
            isCastingSorcery = false;
            aimLazer.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (isCastingSorcery && Input.GetKeyUp(KeyCode.Alpha1) && fireBall_cooldownTimer <= 0f)
        {
            aimLazer.enabled = false;
            SpellSorcery(fireBall);
            fireBall_cooldownTimer = fireBall_cooldown;    // Start counting cooldown time for fire ball.
            aimLazer.enabled = false;    // This is to guarantee that the aiming lazer is cleared.
            isCastingSorcery = false;
        }

        if (isCastingSorcery && Input.GetKeyUp(KeyCode.Alpha2) && fireBall_cooldownTimer <= 0f)
        {
            aimLazer.enabled = false;
            // parabola.ClearVertices();    // *Do NOT call this line!* It causes MissingReferenceException.
            parabola.enabled = false;
            SpellSorcery(snowBall);
            snowBall_cooldownTimer = fireBall_cooldown;    // Start counting cooldown time for fire ball.
            aimLazer.enabled = false;    // This is to guarantee that the aiming lazer is cleared.
            isCastingSorcery = false;
        }
        
        if (Input.GetKeyUp(KeyCode.Alpha5) && bouncingBall_cooldownTimer <= 0f)
        {
            SpellSorcery(bouncingBall);
            bouncingBall_cooldownTimer = bouncingBall_cooldown;    // Start counting cooldown time for bouncing ball.
        }

        UpdateCooldownTimers();
    }

    private void SpellSorcery(GameObject sorcery)
    {
        Vector3 castPosition = transform.position + sorcery.transform.position + camera.transform.forward * 0.4f;
        Quaternion castRotation = camera.transform.rotation;
        
        transform.rotation = camera.transform.rotation * new Quaternion(0, 0, 0, 1);    // Rotates the player towards camera perspective direction.
        transform.Rotate(new Vector3(-transform.rotation.eulerAngles.x, 0, 0));    // Compensate player's pitch so that the player don't appear crooked.
        Instantiate(explosion, castPosition, castRotation);    // Create an explosion effect.
        Instantiate(sorcery, castPosition, castRotation);    // Create the given sorcery object.

        isCastingSorcery = false;
    }

    private void AimFireball()
    {
        Vector3 castPosition = transform.position + fireBall.transform.position + camera.transform.forward * 0.4f;
        
        aimRay.origin = castPosition;
        aimRay.direction = camera.transform.forward;

        aimLazer.SetVertexCount(2);
        aimLazer.SetPosition(0, castPosition);

        if (Physics.Raycast(aimRay, out aimLazerHit, fireBall_range, shootableMask))
        {
            aimLazer.SetPosition(1, aimLazerHit.point);
            GameObject hitObject = aimLazerHit.collider.gameObject;
            if (hitObject.CompareTag("Enemy") && !hitObject.GetComponent<EnemyHealth>().IsDead)
            {
                aimLazer.material.SetColor("_EmissionColor", Color.red);
            }
            else
            {
                aimLazer.material.SetColor("_EmissionColor", Color.cyan);
            }
        }
        else
        {
            aimLazer.SetPosition(1, castPosition + aimRay.direction * fireBall_range);
            aimLazer.material.SetColor("_EmissionColor", Color.cyan);
        }
    }

    private void AimSnowball()
    {
        Vector3 castPosition = transform.position + fireBall.transform.position + camera.transform.forward * 0.4f;
        Vector3 orientation = camera.transform.forward;
        
        parabola.posCompensation = castPosition;
        parabola.orientation = orientation;
    }

    private void UpdateCooldownTimers()
    {
        // for fire balls
        if (fireBall_cooldownTimer > 0f)
        {
            fireBall_cooldownTimer -= Time.deltaTime;
        }
        else
        {
            fireBall_cooldownTimer = 0f;
        }
        
        // for snow balls
        if (snowBall_cooldownTimer > 0f)
        {
            snowBall_cooldownTimer -= Time.deltaTime;
        }
        else
        {
            snowBall_cooldownTimer = 0f;
        }
            
        // for bouncing balls
        if (bouncingBall_cooldownTimer > 0f)
        {
            bouncingBall_cooldownTimer -= Time.deltaTime;
        }
        else
        {
            bouncingBall_cooldownTimer = 0f;
        }
    }
    

    public float GetFireBallCooldownTimer => fireBall_cooldownTimer;  // Important for integrating with UI that displays cooldown information.

    public float GetBouncingBallCooldownTimer => bouncingBall_cooldownTimer;  // Important for integrating with UI that displays cooldown information.
}
