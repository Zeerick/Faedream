using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;

    private Rigidbody rb;

    private int count;

    public int RandomNumber()
    {
        Random random = new Random();
        return Random.Range(-4, 4);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 3;
    }

    private void Update()
    {
        if (count == 0)
        {
            transform.GetComponent<Renderer>().material.color = Color.red;
        }else if (count == 1)
        {
            transform.GetComponent<Renderer>().material.color = new Color(1, (float)0.2877, (float)0.2877);
        }else if (count == 2)
        {
            transform.GetComponent<Renderer>().material.color = new Color(1, (float)0.533, (float)0.533);
        }
        else
        {
            transform.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Tree")) && (count < 3))
        {
            other.GetComponent<Renderer>().material.color = Color.red;
            count = count + 1;
        }else if (other.gameObject.CompareTag("Bucket"))
        {
            other.transform.position = new Vector3(RandomNumber(), (float)0.25, RandomNumber());
            count = 0;
        }
    }
}
