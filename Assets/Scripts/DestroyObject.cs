using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour {

	public float forceRequired = 10.0f;

	private void OnCollisionEnter (Collision col) {
		
		if (col.impulse.magnitude > forceRequired)
		{
			Destroy (gameObject);
		}
		
	}

}
