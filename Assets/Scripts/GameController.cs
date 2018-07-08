using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// GameController controls the overall aspect of the game
// For example, generating tiles, obstacles should be handled here
// Game over, restart actions should also be defined here
public class GameController : MonoBehaviour
{
    
    private GameObject player;
    private PlayerController playerController;
    private NotificationController notificationController;

    // number of lanes
    public int numLanes = 5;                   

    //Swipe manager variables.
    // private bool isTouchInHold = false;
    // private float touchDuration = 0f, scrMid, TouchStart, wid, hig;
    // public float tapDuration;

    // public Button leftSlowButton, rightSlowButton;

    private bool spawned = false, slow = false;

    private int numberOfPlayers;


    public int incgetnop(){
    	numberOfPlayers++;
    	return numberOfPlayers;
    }

    public int getnop(){
    	return numberOfPlayers;
    }

    // Use this for initialization
    void Start()
    {
       
        notificationController = GetComponent<NotificationController>();
        numberOfPlayers = 0;

    }

    // Update is called once per frame
    void Update()
    {
    	//Attaching gamecontroller to the player only after it spawns.
    	if(GameObject.Find("Player(Clone)") != null && !spawned)
    	{
    		player = GameObject.Find("Player(Clone)");
    		//player.name = "a";
    		player.name = "player" + numberOfPlayers;
    		playerController = player.GetComponent<PlayerController>();

    		//Its just a flag, dont worry about it...
    		spawned = true;

    		Debug.Log("Attached game controller to player");
    	}

    	// if(spawned){
    	// 	// if (Input.GetKeyDown(moveL))
    	// 	// {
    	// 	//     playerController.MoveLeft();
    	// 	// }
    	// 	// if (Input.GetKeyDown(moveR))
    	// 	// {
    	// 	//     playerController.MoveRight();
    	// 	// }
    	// 	// if (Input.GetKey(moveSlow))
    	// 	// {
    	// 	//     playerController.MoveSlow();
    	// 	// }

    	// 	//Touch manager.
    	// 	// if (Input.touchCount > 0)
    	// 	// {
    	// 	//     //Get touch event by the first finger.
    	// 	//     Touch myTouch = Input.GetTouch(0);
    	// 	//     //Check If touch is just starting
    	// 	//     if (myTouch.phase == TouchPhase.Began)
    	// 	//     {
    	// 	//         //Reset all related variables
    	// 	//         touchDuration = 0;
    	// 	//         isTouchInHold = false;
    	// 	//         TouchStart = Time.time;

    	// 	//         if(slow != 1){
    	// 	//         	if(myTouch.position.x > scrMid){
    	// 	//         		playerController.MoveRight();
    	// 	//         	}
    	// 	//         	else{
    	// 	//         		playerController.MoveLeft();
    	// 	//         	}
    	// 	//         }
    	// 	//     }

    		    

    	// 	// } //End of Touch Manager.

    		
    	// }
        
    }


   
    public float GetLaneCenterXPos(int laneNum)
    {
        return (float)(laneNum - (numLanes / 2 + 1));
    }

    public void GameOver(string x)
    {
    	Debug.Log("Game Over called bcos: " + x);
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        notificationController.NotifyText("Game Over");
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Scenes/MenuScene");
    }

    public void slowModeOn(){
    	slow = true;
    	//Debug.Log("Slow button pressed!");
    }

    public void slowModeOff(){
    	slow = false;
    	//Debug.Log("Slow button pressed!");
    }

    public bool getSlow(){
        return slow;
    }
}