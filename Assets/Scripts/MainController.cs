using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameController controls the overall aspect of the game
// For example, generating tiles, obstacles should be handled here
// Game over, restart actions should also be defined here
public class MainController : MonoBehaviour {

	public KeyCode moveL;
	public KeyCode moveR;
	public KeyCode moveSlow;

    private GameObject player;
    private PlayerController playerController; 

    private int currentLane;                // current lane
	private int numLanes;                   // number of lanes
	private bool controlLocked = false;     // whether the ball is already moving in a horizontal direction


	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>(); 

        currentLane = 2;
        numLanes = 3; 
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(moveL) && (currentLane > 1) && (controlLocked == false))
		{
			playerController.MoveLeft();
            controlLocked = true; 
            StartCoroutine(stopSlide());
            currentLane = currentLane - 1;
		}
		if (Input.GetKeyDown(moveR) && (currentLane < 3) && (controlLocked == false))
		{
			playerController.MoveRight();
            controlLocked = true; 
            StartCoroutine(stopSlide());
            currentLane = currentLane + 1;
		}
		if (Input.GetKey(moveSlow) && controlLocked == false)
		{
			playerController.MoveSlow();
		}
	}

    // Disable left/right movements for a set time
	IEnumerator stopSlide()
	{
		yield return new WaitForSeconds(.25f);
		playerController.horizVel = 0;
		controlLocked = false;
	}
}
