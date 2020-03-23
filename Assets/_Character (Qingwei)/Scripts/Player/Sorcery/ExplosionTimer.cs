using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTimer : MonoBehaviour
{
    private float timer = 0f;
    
    void FixedUpdate()
    {
        if (timer >= 1.2f)
        {
            UnityEngine.Object.Destroy(gameObject);
            return;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
