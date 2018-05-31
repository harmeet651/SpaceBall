using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour {

    public KeyCode moveSlow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 4);
        if(Input.GetKey(moveSlow))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0.5f);
        }
    }
}
