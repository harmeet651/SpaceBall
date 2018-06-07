using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampController : MonoBehaviour {
    public float speed;

    public bool isMovingUpAndDown;
    public bool isMovingBackAndForth;
    public bool isMovingLeftAndRight;

    private Vector3 beginPos; 
    public int moveAmount;

    private GameObject player; 
    private PlayerController playerController;
    private bool isPlayerOnRamp; 
    
    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player"); 
        playerController = player.GetComponent<PlayerController>(); 

        speed = 1.0f; 

        beginPos = transform.position;
	}

	// Update is called once per frame
	void Update () {
        // Up and down ramp
        if (isMovingUpAndDown)
        {
            transform.position = new Vector3(transform.position.x, beginPos.y + Mathf.PingPong(Time.time * speed, moveAmount), transform.position.z);
        }

        // Back and forth ramp
        if (isMovingBackAndForth)
        {
            speed = 5.0f; 

            transform.position = new Vector3(transform.position.x, transform.position.y, beginPos.z + Mathf.PingPong(Time.time * speed, moveAmount));
        }
        
        // Left and right moving ramp
        if (isMovingLeftAndRight)
        {
            speed = 2.0f;

            float pingpongDistance = Mathf.PingPong(Time.time * speed, moveAmount);

            transform.position = new Vector3(beginPos.x + pingpongDistance, transform.position.y, transform.position.z);
            
            if (isPlayerOnRamp)
            {
                player.transform.position = new Vector3(transform.position.x, player.transform.position.y, player.transform.position.z); 
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            isPlayerOnRamp = true; 

            if (isMovingLeftAndRight)
            {
                playerController.DisableLaneLock(); 
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            isPlayerOnRamp = false;

            if (isMovingLeftAndRight)
            {
                playerController.EnableLaneLock();
            }
        }
    }

    // Destroyer
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
