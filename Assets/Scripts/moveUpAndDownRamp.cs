using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveUpAndDownRamp : MonoBehaviour
{

    public float speed = 1.5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * speed, 4)-1.0f, transform.position.z);
    }
}