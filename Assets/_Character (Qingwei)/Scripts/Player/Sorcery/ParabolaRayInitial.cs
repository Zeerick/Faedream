using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaRayInitial : MonoBehaviour
{
    private GameObject player;
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = player.transform.position + new Vector3(0, 0.5f, 0) + camera.transform.forward * 0.4f;
    }
}
