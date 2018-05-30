using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeMove : MonoBehaviour {

    public float speed= 1.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(Mathf.PingPong(Time.time * speed, 2), transform.position.y, transform.position.z);
	}
}
