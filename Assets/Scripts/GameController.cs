using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameController controls the overall aspect of the game
// For example, generating tiles, obstacles should be handled here
// Game over, restart actions should also be defined here
public class GameController : MonoBehaviour {

	public KeyCode moveL;
	public KeyCode moveR;
	public KeyCode moveSlow;

    private GameObject player;
    private PlayerController playerController;

    public int numLanes = 5;                   // number of lanes

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>(); 
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(moveL))
		{
			playerController.MoveLeft();
		}
		if (Input.GetKeyDown(moveR))
		{
			playerController.MoveRight();
		}
		if (Input.GetKey(moveSlow))
		{
			playerController.MoveSlow();
		}
	}

    public float GetLaneCenterXPos(int laneNum) {
        return (float)(laneNum - (numLanes / 2 + 1));
    }
}
