using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public KeyCode moveL;
    public KeyCode moveR;
    public KeyCode moveSlow;
    private float horizontalTranslateSpeed = 0.06f;
    private float horizSpeed = 4.0f;
    private float horizVelocity = 0;
    private float forwardSpeed = 4.0f;
    private float forwardSlowSpeed = 0.5f;

    private int numLanes; 

    private new Rigidbody rigidbody; 
    private GameController gameController;
    private bool isMovingHorizontal = false; 

    private int currentLane;    // current lane
	private int targetLane;     // target lane to move to
    private float targetXPos;   // target x position

    void Start () {
        // Save a reference to the rigidbody object
        rigidbody = GetComponent<Rigidbody>();

		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

		numLanes = gameController.numLanes;

		// Ball always starts at the center lane
		currentLane = (numLanes / 2) + 1;

		Debug.Log("numLanes=" + numLanes + ", currentLane=" + currentLane);
	}
	
	void Update () {
        rigidbody.velocity = new Vector3(horizVelocity, 0, forwardSpeed);

        Debug.Log("Player pos=" + transform.position);

        // Check if the ball is moving by player inputs (swipe left/right)
        if (isMovingHorizontal) {
            CheckMoveComplete();
        }

        // If the ball is moving on its own
        else {
            float targetXPos = gameController.GetLaneCenterXPos(currentLane);

            // Check x position and adjust accordingly
            float offsetFromCenter = transform.position.x - targetXPos;

            // If the player crossed the border between lanes
            if ((Mathf.Abs(offsetFromCenter) > 0.5) && (currentLane != 1) && (currentLane != numLanes)) {
                if (offsetFromCenter > 0.5) {
                    Debug.Log("currentLane++");
                    currentLane++; 
                }

                else {
                    Debug.Log("currentLane--");
                    currentLane--;
                }
            }

            else {
                horizVelocity = -offsetFromCenter * 4;
            }
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
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

    public void MoveLeft()
    {
        Debug.Log("MoveLeft()"); 

        if ((currentLane > 1) && !isMovingHorizontal) {
            isMovingHorizontal = true;
            horizVelocity = -horizSpeed; 
            targetLane = currentLane - 1;
            targetXPos = gameController.GetLaneCenterXPos(targetLane); 
        }
    }

    public void MoveRight()
    {
        Debug.Log("MoveLeft()");
		
        if ((currentLane < numLanes) && !isMovingHorizontal) {
            isMovingHorizontal = true;
            horizVelocity = horizSpeed; 
            targetLane = currentLane + 1;
            targetXPos = gameController.GetLaneCenterXPos(targetLane);
        }
    }

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

    public void OnHorizontalMoveComplete()
    {
		isMovingHorizontal = false;
		currentLane = targetLane;
        horizVelocity = 0;
	}

    public void MoveSlow()
    {
        rigidbody.velocity = new Vector3(0, 0, forwardSlowSpeed);
    }
}
