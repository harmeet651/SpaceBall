using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveLeftAndRightRamp : MonoBehaviour {

    public float speed= 1.5f;
	private bool isPlayerOnRamp;

	private GameObject player; 
	private PlayerController playerController;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player"); 
		playerController = player.GetComponent<PlayerController>(); 
		isPlayerOnRamp=false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(Mathf.PingPong(Time.time * speed, 4)-2, transform.position.y, transform.position.z);

		if (isPlayerOnRamp)
		{
			player.transform.position = new Vector3(transform.position.x, player.transform.position.y, player.transform.position.z); 
		}

	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			isPlayerOnRamp = true; 

				playerController.DisableLaneLock(); 

		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			isPlayerOnRamp = false;


				playerController.EnableLaneLock();

		}
	}
}
