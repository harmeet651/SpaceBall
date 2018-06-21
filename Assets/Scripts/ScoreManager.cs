using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //initialize score with zero
    private float score = 0.0f;

    //var to send score to unity
    public Text scoreText;

    //var to send high score and keep a count
    public Text highscoreText;
    public float highscoreCount = 0.0f;

    //initial difficulty level
    private int difficultyLevel = 1;

    //max difficulty level
    private int maxDfficultyLevel = 10;

    //score needed to reach next level
    private int scoreToNextLevel = 20;

    PlayerController playerController;

    private bool isInSlowMode;

    // Use this for initialization
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        if (PlayerPrefs.HasKey("HighScore"))
        {
            highscoreCount = PlayerPrefs.GetFloat("HighScore");
        }

        isInSlowMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if player reaches the score needed to move to next level level up
        if (score >= scoreToNextLevel)
            LevelUp();

        //update the score with game time

        if (isInSlowMode == false)
        {
            score += Time.deltaTime;
        }
        else
        {
            score += Time.deltaTime * 0.2f;
        }

        //to calculate and display high Score
        if (score > highscoreCount)
        {
            highscoreCount = score;
            PlayerPrefs.SetFloat("HighScore", highscoreCount);
        }

        //truncate score to integer and convert to string to pass to 'Score-Canvas' component in Unity
        scoreText.text = "Score " + ((int)score).ToString();

        highscoreText.text = "High Score " + ((int)highscoreCount).ToString();
        isInSlowMode = false;
    }

    // Update score by passed amount
    public void AddScore(float amount)
    {
        score += amount;
        highscoreCount += amount;
    }

    //Method to levelup the game
    void LevelUp()
    {
        //if the max difficulty level is reached keep playing at that level
        if (difficultyLevel == maxDfficultyLevel)
            return;

        //exponentially increase score to move to next level. eg 20-40-80...
        scoreToNextLevel *= 3;
        Debug.Log("scoreToNextLevel=" + scoreToNextLevel); 

        difficultyLevel++;

        //fetch the forward velocity of player from 'PlayerController' and update it 
        playerController.AddSpeed(0.5f);
    }

    public void MoveSlow()
    {
        isInSlowMode = true;
    }
}
