using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTutorial : MonoBehaviour {

    // Use this for initialization
    float initialTime;
    // Use this for initialization
    void Start()
    {

        initialTime = Time.time;
        Time.timeScale = 0;

    }

    // Update is called once per frame
    void Update()
    {

        if ((Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) && Time.timeScale == 0)
        {
            Debug.Log("high");
            Time.timeScale = 1;

        }

        if (Time.time - initialTime > 12)
        {
            Debug.Log("check ");
            Application.LoadLevel("TutorialScene8");
        }


    }
}
