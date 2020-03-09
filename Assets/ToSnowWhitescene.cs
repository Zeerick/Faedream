using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToSnowWhitescene : MonoBehaviour
{
    public bool active = false;
    public GameObject InteractionImage;
    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractionImage.SetActive(true);

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
        InteractionImage.SetActive((false));
    }

    public void activate()
    {
        Debug.Log("active");

        Application.LoadLevel("Snow White");
        
    }
}
