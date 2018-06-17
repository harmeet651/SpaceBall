using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int score = 0;

    //Swipe manager variables.
    private int swiped = 0, hold = 0;
    private float touchDuration = 0f, swipeDistance = 0f;


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
        
		//Touch manager.
		if(Input.touchCount > 0) {
			//Get touch event by the first finger.
			Touch myTouch = Input.GetTouch(0);
			//Check If touch is just starting
			if (myTouch.phase == TouchPhase.Began){
				//Reset all related variables
				touchDuration = 0;
				swipeDistance = 0;
				swiped = 0;
				hold = 0; 
			}	
			//Increment total touch duration
			touchDuration += Time.deltaTime;

			//Check for any movement of finger
			if(myTouch.phase == TouchPhase.Moved ){
				Vector2 touchDeltaPosition = myTouch.deltaPosition;
				//Update total distance moved during this swipe/hold.
				swipeDistance += touchDeltaPosition.x;
			}else if(myTouch.phase == TouchPhase.Ended){
				hold = 0;
			}

			//Check if swipe threshold was reached in 0.2ms and if the swipe was not handled before
			if(swipeDistance > 20.0f && touchDuration < 0.1f && swiped == 0){
				playerController.MoveRight();
				//Mark the swipe as handled.
				swiped = 1;
			}else if(swipeDistance < -20.0f && touchDuration < 0.1f && swiped == 0){
				playerController.MoveLeft();
				//Mark the swipe as handled.
				swiped = 1;
			}//Check if player is currently holding and has not swiped during current touch event.
			 //If threshold is not reached within 0.1ms, consider it as a tap and hold.
			 else if(touchDuration > 0.1f && swiped == 0){
				hold = 1;
			}

			if(hold == 1){
				playerController.MoveSlow();
			}

		}//End of Touch Manager.
	}

    public float GetLaneCenterXPos(int laneNum) {
        return (float)(laneNum - (numLanes / 2 + 1));
    }

    public void ChangeScore(int change)
    {
        score += change; 
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Scenes/MenuScene");
    }
}
