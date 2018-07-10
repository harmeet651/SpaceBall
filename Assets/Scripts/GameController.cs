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
    public static float currentMaxPosition = 0, currentMinPosition = 0;

    public static bool gOver = false;

    // number of lanes
    public int numLanes = 5;                   

    private bool spawned = false, slow = false, gameStarted = false;

    public static int numberOfPlayers;


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
        if(GameObject.FindWithTag("Player") != null && !spawned)
            spawned = true;

        if(spawned){
            foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){
                if(player.transform.position.z > currentMaxPosition)
                    currentMaxPosition = player.transform.position.z;
                if(player.transform.position.z < currentMinPosition)
                    currentMinPosition = player.transform.position.z;                    
            }
            //Debug.Log("currentMaxPosition is " + currentMaxPosition );
             if((GameObject.FindGameObjectsWithTag("Player")).Length == 2){
                gameStarted = true;
            }
            if((GameObject.FindGameObjectsWithTag("Player")).Length < 2 && gameStarted){
                Debug.Log("Game Over");
                //(GameObject.FindGameObjectWithTag("GameOverCanvas")).SetActive(true);
                SceneManager.LoadScene("Scenes/MenuScene");

            }
        }
  
    }

   
    public float GetLaneCenterXPos(int laneNum)
    {
        return (float)(laneNum - (numLanes / 2 + 1));
    }

    public void GameOver(string x)
    {
        gOver = true;
    	Debug.Log("Game Over called bcos: " + x);
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        //notificationController.NotifyText("Game Over");
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Scenes/MenuScene");
        //(GameObject.FindGameObjectWithTag("GameOverCanvas")).SetActive(true);
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