using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bucket : MonoBehaviour
{

    public GameObject InteractionImage;

    public Rose hud;

    public treeManager TreeManager1;

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
           // if (active == false)
           // {
            //    active = true;
                activate();          
           // }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractionImage.SetActive(false);
    }

    private void activate()
    {
        if (hud.timeStarted == false)
        {
            GetComponent<Renderer>().material.color = Color.red;
            TreeManager1.paintLeft = 3;
            hud.startTimer();
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.blue;
            TreeManager1.paintLeft = 3;
        }
    }
}
