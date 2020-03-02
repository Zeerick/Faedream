using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bucket : MonoBehaviour
{

    public GameObject InteractionImage;

    public Rose hud;

    public bool active = false;
    // Start is called before the first frame update
    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.E) && other.CompareTag("Player"))
        {
            if (!active)
            {
                activate();
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        InteractionImage.SetActive(false);
    }
    
    private void activate()
    {
        active = true;
        hud.startTimer();
    }
}
