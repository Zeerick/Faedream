using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    public static int damage = 10;  // Damage the ball deals to an enemy.
    private int level = 1;  // Level of the sorcery. To be implemented.
    
    public static float forwardSpeed = 0.5f;  // Horizontal speed of the projectile motion, namely constant speed that the ball goes forward.

    private float timer;
    
    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        if (transform.position.y < -0.1)
        {
            UnityEngine.Object.Destroy(gameObject);
        }
        
        transform.position += transform.forward * forwardSpeed;

        timer += Time.deltaTime;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Environment"))
        {
            
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);  // Deals specified amount of damage to the enemy it hits, then disappears.
        }

        Fade();
    }

    private void Fade()
    {
        /*
        Debug.Log(transform.position);
        Debug.Log(timer);
        */
        
        UnityEngine.Object.Destroy(gameObject);
    }
    
    public int Upgrade()
    {
        level++;
        return level;
    }
}
