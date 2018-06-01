using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public KeyCode moveL;
    public KeyCode moveR;
    public KeyCode moveSlow;
    public float horizVel = 0;

    private int laneNum = 2;                // current lane
    private bool controlLocked = false;     // whether the ball is already moving in a horizontal direction
    private Rigidbody rigidbody; 

    void Start () {
        // Save a reference to the rigidbody object
        rigidbody = GetComponent<Rigidbody>(); 
	}
	
	void Update () {
        rigidbody.velocity = new Vector3(horizVel, 0, 4);

        if (Input.GetKeyDown(moveL) && (laneNum > 1) && (controlLocked == false))
        {
            horizVel = -2;
            StartCoroutine(stopSlide());
            laneNum = laneNum - 1;
            controlLocked = true;
        }
        if (Input.GetKeyDown(moveR) && (laneNum < 3) && (controlLocked == false))
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

     IEnumerator stopSlide()
{
    yield return new WaitForSeconds(.5f);
    horizVel = 0;
    controlLocked = false;
}

    private void OnCollisionEnter(Collision collision)
    {
        
    }

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
}
