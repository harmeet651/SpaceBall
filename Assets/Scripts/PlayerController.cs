using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public KeyCode moveL;
    public KeyCode moveR;
    public KeyCode moveSlow;
    private float horizSpeed = 4.0f;
    private float horizVelocity = 0;
    private float forwardSpeed = 5.0f;
    private float forwardSlowSpeed = 0.5f;

    private int numLanes; 

    private new Rigidbody rigidbody; 
    private GameController gameController;
    private bool isMovingHorizontal = false; 

    private int currentLane;    // current lane
	private int targetLane;     // target lane of horizontal move
    private float targetXPos;   // target x position of horizontal move

    void Start () {
        // Save a reference to the rigidbody object
        rigidbody = GetComponent<Rigidbody>();

        // Save a reference to the main GameController object
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        // Retrieve the number of lanes in this game from GameController
		numLanes = gameController.numLanes;

		// Ball always starts at the center lane
		currentLane = (numLanes / 2) + 1;
	}
	
	void Update () {
        // If the ball is moving, check if movement is complete
        if (isMovingHorizontal) {
            CheckMoveComplete();
        }

        // If the ball is moving on its own
        else {
            float targetXPos = gameController.GetLaneCenterXPos(currentLane);

            // Check x position and adjust accordingly
            float offsetFromCenter = transform.position.x - targetXPos; 

            // If the player crossed the boundary between lanes
            if ((Mathf.Abs(offsetFromCenter) > 0.5) && (currentLane != 1) && (currentLane != numLanes)) {
                // Switch lane to either left or right
                if (offsetFromCenter > 0.5) currentLane++; 
                else currentLane--;
            } 

            else {
                // Move the x position of the player towards the center of the lane
                horizVelocity = -offsetFromCenter * 4;
            }
        }

        rigidbody.velocity = new Vector3(horizVelocity, 0, forwardSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {

		//author:arpit; change: added a death collider for player with traps
		if (collision.gameObject.tag == "death") {
			Destroy (gameObject);
		
		}
    }

    // For adjusting the player's position to the center of the lane
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="enterPipe2")
        {
            rigidbody.velocity= new Vector3(0, 0, 80);
        }
        if (other.gameObject.tag == "exitPipe2")
        {
            rigidbody.velocity = new Vector3(0, 0, 4);
        }
    }

    // Move the player to the left lane
    public void MoveLeft()
    {
        if ((currentLane > 1) && !isMovingHorizontal) {
            isMovingHorizontal = true;
            horizVelocity = -horizSpeed; 
            targetLane = currentLane - 1;
            targetXPos = gameController.GetLaneCenterXPos(targetLane); 
        }
    }

    // Move the player to the right lane
    public void MoveRight()
    {
        if ((currentLane < numLanes) && !isMovingHorizontal) {
            isMovingHorizontal = true;
            horizVelocity = horizSpeed; 
            targetLane = currentLane + 1;
            targetXPos = gameController.GetLaneCenterXPos(targetLane);
        }
    }

    // If the player is in the process of moving, check if lane shifting is complete
    public void CheckMoveComplete()
    {
		// Moving left
		if (currentLane > targetLane)
		{
			if (transform.position.x <= targetXPos)
			{
                OnHorizontalMoveComplete();
            }
		}

		// Moving right
		else if (currentLane < targetLane)
		{
			if (transform.position.x >= targetXPos)
			{
                OnHorizontalMoveComplete();
            }
		}
    }

    // When mov
    public void OnHorizontalMoveComplete()
    {
		isMovingHorizontal = false;
		currentLane = targetLane;
        horizVelocity = 0;
        transform.position = new Vector3(targetXPos, transform.position.y, transform.position.z); 
	}

    public void MoveSlow()
    {
        rigidbody.velocity = new Vector3(0, 0, forwardSlowSpeed);
    }
}
