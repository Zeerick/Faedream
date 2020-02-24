using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class note : MonoBehaviour
{

    public Image noteImage;
    // Start is called before the first frame update
    void Start()
    {
        noteImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {

        if (Input.GetKeyDown((KeyCode.E)) && noteImage.enabled==false && other.CompareTag("Player"))
        {
            ShowNoteImage();
        } 
        else if (Input.GetKeyDown(KeyCode.R) && noteImage.enabled && other.CompareTag("Player"))
        {
            HideNoteImage();
        }
    }

    public void HideNoteImage()
    {
        noteImage.enabled = false;
    }

    public void ShowNoteImage()
    {
        noteImage.enabled = true;
    }
}

