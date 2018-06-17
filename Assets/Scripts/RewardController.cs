using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    private GameObject gameControllerObject;
    private GameController gameController;

    public int rewardAmount; 
    public bool isSpinning;
    private bool isInMagneticField = false;

    private GameObject player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");

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

        if (isInMagneticField)
        {
            float speed = 15.0f;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            gameController.ChangeScore(rewardAmount);
            Destroy(gameObject);
        }

        else if (col.gameObject.tag == "MagneticField")
        {
            isInMagneticField = true; 
        }
    }
}