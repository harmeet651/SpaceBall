using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBackAndForthRamp : MonoBehaviour
{

    public float speed = 1.5f;

	public Rigidbody rb;
    // Use this for initialization
    void Start()
    {
		rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		float temp = rb.transform.position.z;
		transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(Time.time * speed, 5) + 10 );
		Debug.Log ("Position:" + temp);
	}
}