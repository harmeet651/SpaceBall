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

        if (Input.GetKeyDown(moveL) && (laneNum > 0) && (controlLocked == false))
        {
            horizVel = -2;
            StartCoroutine(stopSlide());
            laneNum = laneNum - 1;
            controlLocked = true;
        }
        if (Input.GetKeyDown(moveR) && (laneNum < 4) && (controlLocked == false))
        {
            horizVel = 2;
            StartCoroutine(stopSlide());
            laneNum = laneNum + 1;
            controlLocked = true;
        }
        if(Input.GetKey(moveSlow) && controlLocked == false)
        {
            rigidbody.velocity = new Vector3(horizVel, 0, 0.5f);
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        
    }

    // For adjusting the player's position to the center of the lane
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="goBack")
        {
           // transform.position = Vector3.Lerp()
            transform.position = new Vector3(1, 0.8f, 5);
        }
        if (other.gameObject.tag == "goBackLeft")
        {
            transform.position = new Vector3(0, 0.8f, 5);
        }
        if (other.gameObject.tag == "goBackRight")
        {
            transform.position = new Vector3(2, 0.8f, 5);
        }
        if (other.gameObject.tag == "set2middle")
        {
            transform.position = new Vector3(1, 0.8f, 18);
        }
        if (other.gameObject.tag == "set2left")
        {
            transform.position = new Vector3(0, 0.8f, 18);
        }
        if (other.gameObject.tag == "set2right")
        {
            transform.position = new Vector3(2, 0.8f, 18);
        }
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
