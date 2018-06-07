using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Score : MonoBehaviour {

	//initialize score with zero
	private float score = 0.0f;

	//var to send score to unity
	public Text scoreText;

	//initial difficulty level
	private int difficultyLevel = 1;

	//max difficulty level
	private int maxDfficultyLevel = 10;

	//score needed to reach next level
	private int scoreToNextLevel = 10;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		//if player reaches the score needed to move to next level level up
		if (score >= scoreToNextLevel)
			LevelUp ();

		//update the score with game time
		score += Time.deltaTime;

		//truncate score to integer and convert to string to pass to 'Score-Canvas' component in Unity
		scoreText.text = ((int)score).ToString ();

	}

	//Method to levelup the game
	void LevelUp()
	{
		//if the max difficulty level is reached keep playing at that level
		if (difficultyLevel == maxDfficultyLevel)
			return;

		//exponentially increase score to move to next level. eg 20-40-80...
		scoreToNextLevel *= 2;
		difficultyLevel++;

		//fetch the forward velocity of player from 'PlayerController' and update it 
		GetComponent<PlayerController>().SetSpeed (difficultyLevel);

		//this piece of code is useless and is just used to log whether the player speed actually increases
		float flag = GetComponent<PlayerController> ().SendSpeed ();
		Debug.Log (flag);
	}
}
