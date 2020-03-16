using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToSnowWhitescene : MonoBehaviour
{
    public bool active = false;


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.CompareTag("Player"))
        {
            Debug.Log("active");
            if (!active)
            {
                activate();
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
    }

    public void activate()
    {
        Debug.Log("active");

        Application.LoadLevel("Snow White");
        
    }
}
