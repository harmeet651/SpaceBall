using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueStarController : MonoBehaviour
{
    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Equals("Player"))
        {
            rb.isKinematic = false;
            rb.velocity = new Vector3(0, 0, 4);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Equals("Player"))
        {
            Debug.Log("Got ya!");
            Destroy(gameObject);
        }
    }
}

