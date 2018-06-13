using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour {

    private GameController gameController;

    // Use this for initialization
    void Start () {
        // Save a reference to the main GameController object
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            //gameController.GameOver(); 
        }
    }

    // Destroyer
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
