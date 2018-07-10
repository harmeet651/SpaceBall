using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Threading;

public class MoveRightTutorial : MonoBehaviour {

    float initialTime;
	// Use this for initialization
	void Start () {

        initialTime = Time.time;
        Time.timeScale = 0;
      
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.D) && Time.timeScale == 0)
        {
            Debug.Log("high");
            Time.timeScale = 1;
               
        }

        if (Input.touchCount > 0)
        {
            //Get touch event by the first finger.
            Touch myTouch = Input.GetTouch(0);
            //Check If touch is just starting
            if (myTouch.phase == TouchPhase.Began)
            { 
                if (myTouch.position.x > (Screen.width/2)) {
                    Time.timeScale = 1;
                }
               
            }
        }






        if (Time.time - initialTime > 4)
        {
            Debug.Log("check ");
            Application.LoadLevel("TutorialScene2");
        }

     
    }
}
