using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    // Player game object
    private GameObject player;

    // Distance between the player and the camera
    private Vector3 offset; 

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");

        // Store the initial distance between the player and the camera
        offset = new Vector3(0, transform.position.y - player.transform.position.y, transform.position.z - player.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {

    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
