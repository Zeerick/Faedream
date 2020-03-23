using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirrorCam : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject mirror;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Reflect(mainCamera.transform.position, mirror.transform.eulerAngles);
    }
}
