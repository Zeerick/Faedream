using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class BouncingBall : MonoBehaviour
{
    public static int damage = 10;  // Damage the ball deals to an enemy.
    private int level = 1;  // Level of the sorcery. To be implemented.
    
    public static bool isRealistic = false;  // DO NOT SWITCH ON!!!
    public static float forwardSpeed = 0.05f;  // Horizontal speed of the projectile motion, namely constant speed that the ball goes forward.
    public static float bouncingForce = 25f;  // Vertical acceleration of the projectile motion, namely how hard the ball bounces up.
    public static int maxBouncingTime = 3;  // Times that each ball bounces. Reduce by 1 for each time of bouncing. When set to 0, the ball will disappear.
    
    public static float forwardForce = 0.05f;  // Horizontal initial force of the projectile motion (in realistic mode).
    public static float bouncingAttenuation = 0.5f;  // Attenuation of bouncing force each time the ball bounces (in realistic mode).

    private int bouncingTime = maxBouncingTime;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isRealistic)
        {
            rb.AddForce(transform.rotation.eulerAngles * forwardForce);  // In realistic mode, the ball starts with a force whose direction is relative to a flexible direction.
        }
        else
        {
            transform.position += transform.forward * forwardSpeed;
        }

        if (transform.position.y < -0.1)
        {
            UnityEngine.Object.Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            // other.gameObject.GetComponent<EnemyHealth>().takeDamage(damage);  // Deals specified amount of damage to the enemy it hits, then disappears.
            Fade();
            return;
        }
        
        if (other.gameObject.tag.Equals("Environment"))
        {
            if (bouncingTime <= 0)
            {
                Fade();
                return;
            }
            
            bouncingTime --;
            
            if (isRealistic)
            {
                rb.AddForce(0, bouncingForce, 0);
                bouncingForce = bouncingForce * bouncingAttenuation;  // In realistic mode, the ball bounces with attenuating energy.
            }
            else
            {
                rb.AddForce(0, bouncingForce, 0);
            }
        }
    }

    private void Fade()
    {
        UnityEngine.Object.Destroy(gameObject);
    }

    public int Upgrade()
    {
        level++;
        return level;
    }
}
