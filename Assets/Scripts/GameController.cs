using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// GameController controls the overall aspect of the game
// For example, generating tiles, obstacles should be handled here
// Game over, restart actions should also be defined here
public class GameController : MonoBehaviour
{
    public KeyCode moveL;
    public KeyCode moveR;
    public KeyCode moveSlow;

    private GameObject player;
    private PlayerController playerController;

    public int numLanes = 5;                   // number of lanes
    public int score = 0;
    public int highscoreCount = 0;

    //Swipe manager variables.
    private int hold = 0;
    private float touchDuration = 0f, scrMid, TouchStart;

    public float tapDuration;


	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>(); 

        scrMid = (float) Screen.width;
        scrMid = scrMid /2;
        tapDuration = 0.165f;
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
				hold = 0; 
				TouchStart = Time.time;
			}	
			//Increment total touch duration
			touchDuration += Time.deltaTime;

			if(hold == 1){
				playerController.MoveSlow();
			}else{
				if(myTouch.phase == TouchPhase.Ended){
					hold = 0;
					Debug.Log("Touch: Duration is " + (Time.time-TouchStart));
					if(myTouch.phase == TouchPhase.Ended && (Time.time-TouchStart) < tapDuration){
						Debug.Log("Touch: TAP, Last Time.DeltaTime is " + Time.deltaTime);
						if(myTouch.position.x > scrMid)
							playerController.MoveRight();
						else
							playerController.MoveLeft();
						//Mark the swipe as handled.
					}
				}
				else if((Time.time-TouchStart) > tapDuration){
					hold = 1;
					Debug.Log("Touch: HOLD" + (Time.time-TouchStart) + " touchduration: " + touchDuration);
					
				}
			}
		}//End of Touch Manager.
	}

    public float GetLaneCenterXPos(int laneNum)
    {
        return (float)(laneNum - (numLanes / 2 + 1));
    }

    public void ChangeScore(int change)
    {
        score += change; 
        highscoreCount += change; 
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Scenes/MenuScene");
    }
}