using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public KeyCode moveL;
    public KeyCode moveR;
    public KeyCode moveSlow;
    private float horizSpeed = 10.0f;
    private float horizVelocity = 0;
    private float forwardSpeed = 12.0f;
    private float forwardSlowSpeed = 0.5f;

    private int numLanes;       // number of lanes

    private new Rigidbody rigidbody; 
    private GameController gameController;
    private bool isMovingHorizontal = false;
    private bool isLaneLockEnabled = true; 

    private int currentLane;    // current lane
	private int targetLane;     // target lane of horizontal move
    private float targetXPos;   // target x position of horizontal move

	public Transform explodeObj;	//effect after collision with trap

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
			Instantiate (explodeObj, transform.position, explodeObj.rotation);
		
		}
    }

    void LateUpdate()
    {
        if (transform.position.y < -5)
        {
            gameController.GameOver();
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

    public void EnableLaneLock()
    {
        isLaneLockEnabled = true; 
    }

    public void DisableLaneLock()
    {
        isLaneLockEnabled = false; 
    }

    // Clean up after horizontal move has been complete
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

    // Getter for forward moving speed
    public float GetSpeed()
    {
        return forwardSpeed; 
    }

	// Setter for forward moving speed
	public void SetSpeed(float speed)
	{
        forwardSpeed = speed; 
	}

	//this piece of code is useless and is just used to log whether the speed of the player actually increases
	public float SendSpeed()
	{
		return forwardSpeed;
	}
}
