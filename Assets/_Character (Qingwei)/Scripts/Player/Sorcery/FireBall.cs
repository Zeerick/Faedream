using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public static int damage = 20;    // Damage the ball deals to an enemy. 
    private int level = 1;    // Level of the sorcery. To be implemented.
    
    public static float forwardSpeed = 0.5f;    // Constant speed which the ball moves forward. 
    public static float range = 30;    // Maximum distance which the ball can travel.
    public static float expansion = 1.01f;    // Coefficient of expansion of the ball when it flies forward. 1 stands for no change in scale.

    private float travelledDistance;

    public GameObject explosion;

    private void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (travelledDistance >= range)
        {
            Fade();
        }
        
        transform.localScale = transform.localScale * expansion;
        
        transform.position += transform.forward * forwardSpeed;
        travelledDistance += forwardSpeed * Time.deltaTime * 100;
        
        if (transform.position.y < -0.1)
        {
            Fade();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);    // Deals specified amount of damage to the enemy it hits, then disappears.
        }
        else if (other.gameObject.CompareTag("Environment"))
        {
            // To be implemented if needed.
        }

        Fade();
    }

    private void Fade()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        UnityEngine.Object.Destroy(gameObject);
    }
    
    public int Upgrade()
    {
        level++;
        return level;
    }
}
