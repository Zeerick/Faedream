using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsTree : MonoBehaviour
{
    
    // bug - if you try to acxtive a tree before timer starts, then activate the timer, you cant asctive a tree

    public bool active = false;
    public GameObject InteractionImage;

    private treeManager parentScript;
    // Start is called before the first frame update
    void Start()
    {
        parentScript = transform.GetComponentInParent<treeManager>();
    }

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
        if (Input.GetKeyDown(KeyCode.E) && other.CompareTag("Player") && parentScript.hud.timeStarted)
        {
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

        if (parentScript.paintLeft > 0)
        {

            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("transform");
            active = true;
            parentScript.paintLeft = parentScript.paintLeft - 1;
            parentScript.checkStatus(); //call from here?
        }
        else
        {
            
        }
        
    }

    
}
