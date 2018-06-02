using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public KeyCode moveL;
    public KeyCode moveR;
    public KeyCode moveSlow;
    public float horizVel = 0; 

   private new Rigidbody rigidbody; 

    void Start () {
        // Save a reference to the rigidbody object
        rigidbody = GetComponent<Rigidbody>(); 
	}
	
	void Update () {
        rigidbody.velocity = new Vector3(horizVel, 0, 4);

        Debug.Log("Player pos=" + transform.position);
	}

    private void OnCollisionEnter(Collision collision)
    {
    }

    // For adjusting the player's position to the center of the lane
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="enterPipe2")
        {
            rigidbody.velocity= new Vector3(horizVel, 0, 80);
        }
        if (other.gameObject.tag == "exitPipe2")
        {
            rigidbody.velocity = new Vector3(horizVel, 0, 4);
        }
    }

    public void MoveLeft()
    {
		horizVel = -2;
    }

    public void MoveRight()
    {
		horizVel = 2;
    }

    public void MoveSlow()
    {
        rigidbody.velocity = new Vector3(horizVel, 0, 0.5f);
    }
}
