using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roseCollider : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("character"))
        {
            if (GetComponent<Renderer>().material.color == Color.white)
            {
                transform.GetComponent<Renderer>().material.color = Color.red;
            }
        }
        transform.GetComponent<Renderer>().material.color = Color.red;
    }
}
