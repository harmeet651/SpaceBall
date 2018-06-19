using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum HorizontalMovement
{
    None, 
    Left, 
    Right
}

public class PlayerController : MonoBehaviour
{
    public int rewards = 0;

    public KeyCode moveL;
	public KeyCode moveR;
	public KeyCode moveSlow;
	private float horizSpeed = 10.0f;
	private float horizVelocity = 0;
	private float forwardSpeed = 10.0f;
	private float forwardSlowSpeed = 0.2f;
    public float verticalVelocity = 0.0f;
    public bool isFlying = false; 

	private int numLanes;

	private Rigidbody rb;
	private GameController gameController;
	private HorizontalMovement horizontalMoveStatus = HorizontalMovement.None;
	private bool isLaneLockEnabled = true;

	private int currentLane;    // current lane
	private int targetLane;     // target lane of horizontal move
	private float targetXPos;   // target x position of horizontal move

    public GameObject magneticField;
    private MagneticFieldController magneticFieldController; 

	public Transform explodeObj;    //effect after collision with trap

	void Start()
	{
		// Save a reference to the rigidbody object
		rb = GetComponent<Rigidbody>();

		// Save a reference to the main GameController object
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        // Magnetic field gameobject
        magneticFieldController = magneticField.GetComponent<MagneticFieldController>(); 

        // Retrieve the number of lanes in this game from GameController
        numLanes = gameController.numLanes;

		// Ball always starts at the center lane
		currentLane = (numLanes / 2) + 1;
        targetLane = currentLane; 
	}

	void Update()
	{
        // If the ball is moving, check if movement is complete
        if (horizontalMoveStatus != HorizontalMovement.None)
		{
			CheckMoveComplete();
		}

		// If the ball is moving on its own
		else
		{
			float targetXPos = gameController.GetLaneCenterXPos(currentLane);

			// Check x position and adjust accordingly
			float offsetFromCenter = transform.position.x - targetXPos;

			// If the player crossed the boundary between lanes
			if ((Mathf.Abs(offsetFromCenter) > 0.5) && (currentLane != 1) && (currentLane != numLanes))
			{
				// Switch lane to either left or right
				if (offsetFromCenter > 0.5) currentLane++;
				else currentLane--;
			}

			else
			{
				if (isLaneLockEnabled)
				{
					// Move the x position of the player towards the center of the lane
					horizVelocity = -offsetFromCenter * 4;
				}
			}
		}

        if (isFlying)
        {
            rb.velocity = new Vector3(horizVelocity, verticalVelocity, forwardSpeed * 3); 
        }

        else
        {
            rb.velocity = new Vector3(horizVelocity, verticalVelocity, forwardSpeed);
        }
	}

    void LateUpdate()
    {
        // If the player falls below -5.0f in y axis, game over
        if (transform.position.y <= -5.0f)
        {
            gameController.GameOver();
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
        
		if (collision.gameObject.tag == "death" && !isFlying)
		{
			Destroy(gameObject);
			Instantiate(explodeObj, transform.position, explodeObj.rotation);
            gameController.GameOver(); 
		}

        else if (collision.gameObject.tag == "speedAddRampToBall")
        {
            forwardSlowSpeed = 1.2f;
        }

        else
        {
            forwardSlowSpeed = 0.2f;
        }
	}

	void OnTriggerEnter(Collider col)
	{
        if (col.gameObject.tag == "MagnetItem")
        {
            Destroy(col.gameObject);
            EnableMagneticField();
        }
            if (col.gameObject.name.Contains("myFly"))
            {
            Fly();
            //SampleParabola(new Vector3(transform.position.x,transform.position.y,transform.position.z), new Vector3(transform.position.x, transform.position.y, transform.position.z+40), 40, 0);
                Destroy(col.gameObject);
            }
    }

	// Move the player to the left lane
	public void MoveLeft()
	{
        if ((currentLane > 1) && horizontalMoveStatus != HorizontalMovement.Left)
		{
			horizontalMoveStatus = HorizontalMovement.Left;
			horizVelocity = -horizSpeed;
			targetLane = targetLane - 1;
			targetXPos = gameController.GetLaneCenterXPos(targetLane);
		}
	}

	// Move the player to the right lane
	public void MoveRight()
	{
		if ((currentLane < numLanes) && horizontalMoveStatus != HorizontalMovement.Right)
		{
			horizontalMoveStatus = HorizontalMovement.Right;
			horizVelocity = horizSpeed;
			targetLane = targetLane + 1;
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

	// Event handler when moving to a different lane is complete
	public void OnHorizontalMoveComplete()
	{
		horizontalMoveStatus = HorizontalMovement.None;
		currentLane = targetLane;
		horizVelocity = 0;
		transform.position = new Vector3(targetXPos, transform.position.y, transform.position.z);
	}

	public void MoveSlow()
	{
		rb.velocity = new Vector3(0, 0, forwardSlowSpeed);
	}
    
	public void EnableLaneLock()
	{
		isLaneLockEnabled = true;
	}

	public void DisableLaneLock()
	{
		isLaneLockEnabled = false;
	}

    public void EnableMagneticField()
    {
        EnableMagneticField(5.0f, true); 
    }

    public void EnableMagneticField(float magneticFieldSize, bool automaticDisable)
    {
        Debug.Log("EnableMagneticField(magneticFieldSize=" + magneticFieldSize + ")");

        magneticField.SetActive(true); 
        magneticFieldController.originalScale = new Vector3(magneticFieldSize, magneticFieldSize, magneticFieldSize);

        // If automatic disable option after x seconds is on, start a corutine
        if (automaticDisable)
        {
            StartCoroutine(MagneticFieldCoroutine());
        }
    }

    public void DisableMagneticField()
    {
        magneticField.SetActive(false);
    }

    IEnumerator MagneticFieldCoroutine()
    {
        yield return new WaitForSeconds(10);
        DisableMagneticField(); 
    }
    
	public void AddSpeed(float modifier)
	{
		forwardSpeed = forwardSpeed + modifier;
	}
    
	public float GetSpeed()
	{
		return forwardSpeed;
	}

    // Start flying
    public void Fly()
    {
        // Actual flying motion is done through FlightController
        GetComponent<FlightController>().Fly();
        EnableMagneticField(20.0f, false); 
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(100, 100, 100, 20), "Rewards : " + rewards);
        if (rewards == 5)
        {
            rewards = 1;
        }
    }
}