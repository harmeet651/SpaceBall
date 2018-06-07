using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    private GameObject gameControllerObject;
    private GameController gameController;

    public int rewardAmount; 
    public bool isSpinning; 

    // Use this for initialization
    void Start()
    {
        gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(new Vector3(3, 3, 3));
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            gameController.AddScore(rewardAmount);
            Destroy(gameObject);
        }
    }

    // Destroyer
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}