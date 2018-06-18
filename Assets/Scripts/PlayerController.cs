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

	private int numLanes;

	private new Rigidbody rigidbody;
	private GameController gameController;
	private HorizontalMovement horizontalMoveStatus = HorizontalMovement.None;
	private bool isLaneLockEnabled = true;

	private int currentLane;    // current lane
	private int targetLane;     // target lane of horizontal move
	private float targetXPos;   // target x position of horizontal move

    public GameObject magneticSphere; 

	public Transform explodeObj;    //effect after collision with trap

	void Start()
	{
		// Save a reference to the rigidbody object
		rigidbody = GetComponent<Rigidbody>();

		// Save a reference to the main GameController object
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

		// Retrieve the number of lanes in this game from GameController
		numLanes = gameController.numLanes;

		// Ball always starts at the center lane
		currentLane = (numLanes / 2) + 1;
        targetLane = currentLane; 
	}

	void Update()
	{
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space");
            rigidbody.AddForce(new Vector3(0, 40000, 40000));
        }

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

		rigidbody.velocity = new Vector3(horizVelocity, 0, forwardSpeed);
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
        
		if (collision.gameObject.tag == "death")
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
		rigidbody.velocity = new Vector3(0, 0, forwardSlowSpeed);
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
        magneticSphere.SetActive(true);
        StartCoroutine(DisableMagneticField());
    }

    IEnumerator DisableMagneticField()
    {
        yield return new WaitForSeconds(10);
        magneticSphere.SetActive(false);
    }
    
	public void AddSpeed(float modifier)
	{
		forwardSpeed = forwardSpeed + modifier;
	}
    
	public float GetSpeed()
	{
		return forwardSpeed;
	}
    public void OnGUI()
    {
        GUI.Label(new Rect(100, 100, 100, 20), "Rewards : " + rewards);
        if (rewards == 5)
        {
            rewards = 1;
        }
    }
    //Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    //{
    //    Debug.Log("hhudhuehduehdhe");
    //    float parabolicT = t * 2 - 1;
    //    if (Mathf.Abs(start.y - end.y) < 0.1f)
    //    {
    //        //start and end are roughly level, pretend they are - simpler solution with less steps
    //        Vector3 travelDirection = end - start;
    //        Vector3 result = start + t * travelDirection;
    //        result.y += (-parabolicT * parabolicT + 1) * height;
    //        return result;
    //    }
    //    else
    //    {
    //        //would generally not happen
    //        //start and end are not level, gets more complicated
    //        Vector3 travelDirection = end - start;
    //        Vector3 levelDirecteion = end - new Vector3(start.x, end.y, start.z);
    //        Vector3 right = Vector3.Cross(travelDirection, levelDirecteion);
    //        Vector3 up = Vector3.Cross(right, travelDirection);
    //        if (end.y > start.y) up = -up;
    //        Vector3 result = start + t * travelDirection;
    //        result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
    //        return result;
    //    }

    //}
}