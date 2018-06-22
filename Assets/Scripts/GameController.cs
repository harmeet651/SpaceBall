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
    public KeyCode moveL;
    public KeyCode moveR;
    public KeyCode moveSlow;

    private GameObject player;
    private PlayerController playerController;
    private ScoreManager scoreManager;
    private NotificationController notificationController; 

    public int numLanes = 5;                   // number of lanes

    //Swipe manager variables.
    private bool hold = false;
    private float touchDuration = 0f, scrMid, TouchStart;

    public float tapDuration;

    public Canvas tutorialCanvas;
    public GameObject leftInstructionSlide;
    public GameObject rightInstructionSlide;
    public GameObject holdInstructionSlide;
    public Text countDownText; 


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        scoreManager = player.GetComponent<ScoreManager>();
        notificationController = GetComponent<NotificationController>(); 
        scrMid = (float)Screen.width;
        scrMid = scrMid / 2;
        tapDuration = 0.165f;

        StartCoroutine(InitialTutorial());
    }
    
    IEnumerator InitialTutorial()
    {
        yield return new WaitForSeconds(3.0f);
        leftInstructionSlide.SetActive(false);
        rightInstructionSlide.SetActive(true);

        yield return new WaitForSeconds(2.5f);
        rightInstructionSlide.SetActive(false);
        holdInstructionSlide.SetActive(true);

        yield return new WaitForSeconds(3.0f);
        holdInstructionSlide.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        
        for (int secondsLeft = 3; secondsLeft > 0; secondsLeft--)
        {
            countDownText.text = "" + secondsLeft; 
            yield return new WaitForSeconds(0.8f); 
        }

        tutorialCanvas.enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
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
            scoreManager.MoveSlow();
        }

        //Touch manager.
        if (Input.touchCount > 0)
        {
            //Get touch event by the first finger.
            Touch myTouch = Input.GetTouch(0);
            //Check If touch is just starting
            if (myTouch.phase == TouchPhase.Began)
            {
                //Reset all related variables
                touchDuration = 0;
                hold = false;
                TouchStart = Time.time;
            }
            //Increment total touch duration
            touchDuration += Time.deltaTime;

            if (hold == true)
            {
                playerController.MoveSlow();
                scoreManager.MoveSlow();
            }
            else
            {
                if (myTouch.phase == TouchPhase.Ended)
                {
                    hold = false;
                    if (myTouch.phase == TouchPhase.Ended && (Time.time - TouchStart) < tapDuration)
                    {
                        if (myTouch.position.x > scrMid)
                            playerController.MoveRight();
                        else
                            playerController.MoveLeft();
                        //Mark the swipe as handled.
                    }
                }
                else if ((Time.time - TouchStart) > tapDuration)
                {
                    hold = true;
                    playerController.MoveSlow();
                    scoreManager.MoveSlow();

                }
            }
        }//End of Touch Manager.
    }

    public float GetLaneCenterXPos(int laneNum)
    {
        return (float)(laneNum - (numLanes / 2 + 1));
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine()); 
    }

    IEnumerator GameOverCoroutine()
    {
        notificationController.NotifyText("Game Over");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Scenes/MenuScene");
    }
}