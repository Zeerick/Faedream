using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackX : MonoBehaviour
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

    // private bool isCastingSorcery = false;
    private enum Sorcery
    {
        none, fireBall, snowBall, bouncingBall
    }
    private Sorcery castingSorcery = Sorcery.none;
    
    private LayerMask shootableMask;
    private LineRenderer fireball_aimLazer;
    
    private Ray aimRay;
    private RaycastHit aimLazerHit;

    private LineRenderer snowball_aimLazer;
    private ParabolaRay parabola;
    
    // public ParticleSystem.Particle explosion;
    
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");    // Automatically assign the variable to the main camera. This requires the main camera to have tag "MainCamera".
        shootableMask = LayerMask.GetMask("Shootable");
        fireball_aimLazer = GetComponent<LineRenderer>();
        snowball_aimLazer = GetComponentsInChildren<LineRenderer>()[1];
        parabola = GetComponentInChildren<ParabolaRay>();
    }

    private void Update()
    {
        // Displays fire ball aiming effect
        if (castingSorcery == Sorcery.fireBall)
        {
            AimFireball();
            fireball_aimLazer.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && fireBall_cooldownTimer <= 0f)
        {
            if (castingSorcery == Sorcery.fireBall)
            {
                clear();
            }
            else
            {
                castingSorcery = Sorcery.fireBall;
                AimFireball();
                fireball_aimLazer.enabled = true;
            }
        }
        
        // Displays snow ball aiming effect
        if (castingSorcery == Sorcery.snowBall)
        {
            AimSnowball();
            parabola.enabled = true;
            snowball_aimLazer.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && snowBall_cooldownTimer <= 0f)
        {
            if (castingSorcery == Sorcery.snowBall)
            {
                clear();
            }
            else
            {
                castingSorcery = Sorcery.snowBall;
                AimSnowball();
                parabola.enabled = true;
                snowball_aimLazer.enabled = true;
            }
        }

        // Allows cancelling sorceries when targeting them, by pressing right mouse button.
        if (castingSorcery != Sorcery.none && Input.GetMouseButtonDown(1))
        {
            castingSorcery = Sorcery.none;
            clear();
        }
    }

    void FixedUpdate()
    {
        
        if (castingSorcery != Sorcery.none && Input.GetMouseButtonDown(0))
        {
            if (castingSorcery == Sorcery.fireBall && fireBall_cooldownTimer <= 0f)
            {
                SpellSorcery(fireBall);
                fireBall_cooldownTimer = fireBall_cooldown;    // Start counting cooldown time for fire ball.
                clear();
            }
    
            if (castingSorcery == Sorcery.snowBall && snowBall_cooldownTimer <= 0f)
            {
                SpellSorcery(snowBall);
                snowBall_cooldownTimer = fireBall_cooldown;    // Start counting cooldown time for fire ball.
                clear();
            }
            
            if (castingSorcery == Sorcery.bouncingBall && bouncingBall_cooldownTimer <= 0f)
            {
                SpellSorcery(bouncingBall);
                bouncingBall_cooldownTimer = bouncingBall_cooldown;    // Start counting cooldown time for bouncing ball.
                clear();
            }
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

        castingSorcery = Sorcery.none;
    }

    private void AimFireball()
    {
        Vector3 castPosition = transform.position + fireBall.transform.position + camera.transform.forward * 0.4f;
        
        aimRay.origin = castPosition;
        aimRay.direction = camera.transform.forward;

        // aimLazer.SetVertexCount(2);
        fireball_aimLazer.SetPosition(0, castPosition);

        if (Physics.Raycast(aimRay, out aimLazerHit, fireBall_range, shootableMask))
        {
            fireball_aimLazer.SetPosition(1, aimLazerHit.point);
            GameObject hitObject = aimLazerHit.collider.gameObject;
            if (hitObject.CompareTag("Enemy") && !hitObject.GetComponent<EnemyHealth>().IsDead)
            {
                fireball_aimLazer.material.SetColor("_EmissionColor", Color.red);
            }
            else
            {
                fireball_aimLazer.material.SetColor("_EmissionColor", Color.cyan);
            }
        }
        else
        {
            fireball_aimLazer.SetPosition(1, castPosition + aimRay.direction * fireBall_range);
            fireball_aimLazer.material.SetColor("_EmissionColor", Color.cyan);
        }
    }

    private void AimSnowball()
    {
        Vector3 castPosition = transform.position + fireBall.transform.position + camera.transform.forward * 0.4f;
        Vector3 orientation = camera.transform.forward;
        
        parabola.posCompensation = castPosition;
        parabola.orientation = orientation;
    }
    
    /// <summary>
    /// This method clears up parabola, line renderer and castingSorcery state when necessary.
    /// </summary>
    private void clear()
    {
        castingSorcery = Sorcery.none;
        fireball_aimLazer.enabled = false;
        snowball_aimLazer.enabled = false;
        // parabola.ClearVertices();
        parabola.enabled = false;
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
