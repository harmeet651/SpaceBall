using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBackAndForthRamp : MonoBehaviour
{

    public float speed = 1.0f;
    private float startingPosition = 0;

    // Use this for initialization
    void Start()
    {
        startingPosition = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(Time.time * speed, 5) + startingPosition);
    }
}
