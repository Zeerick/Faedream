using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeManager : MonoBehaviour
{
    public Rose hud;
    private int childCount =0;
    private int childrenActive = 0;
    private bool complete;
    public int paintLeft = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            childCount += 1;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
    }
    

    public void checkStatus()
    {
        complete = true;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<HeartsTree>().active == false)
            {
                complete = false;
            }
        }

        if (complete)
        {
            updateHUDSuccess();
        }
        else
        {
            updateHUDAdd();
        }
    }

    public void updateHUDSuccess()
    {
        childrenActive += 1;
        hud.add(childrenActive, childCount);
        hud.win();
    }

    public void updateHUDAdd()
    {
        childrenActive += 1;
        
        if(childrenActive <= childCount)
        {
         hud.add(childrenActive, childCount);   
        }

    }

    
    
}
