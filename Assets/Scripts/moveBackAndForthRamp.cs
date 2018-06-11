using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBackAndForthRamp : MonoBehaviour
{

    public float speed = 1.5f;
    public float initialpos;

    // Use this for initialization
    void Start()
    {
        initialpos = transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(Time.time * speed, 5)-(initialpos+20) );
        Debug.Log(initialpos+"created at");
	}
}