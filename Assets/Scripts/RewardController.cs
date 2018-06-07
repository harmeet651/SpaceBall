using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    private GameObject gameControllerObj;
    private ScoreManager scoreManager;

    public int rewardAmount; 
    public bool isSpinning; 

    // Use this for initialization
    void Start()
    {
        gameControllerObj = GameObject.FindWithTag("GameController");
        scoreManager = gameControllerObj.GetComponent<ScoreManager>();
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
            scoreManager.AddScore(rewardAmount);
            Destroy(gameObject);
        }
    }

    // Destroyer
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}