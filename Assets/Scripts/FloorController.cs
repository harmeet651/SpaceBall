using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour {
    // Destroyer
    void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}