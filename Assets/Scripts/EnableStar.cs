using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableStar: MonoBehaviour {

	private Light rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Light>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Space))
        {
            rb.enabled = !rb.enabled;
        }

        
		
	}
}
