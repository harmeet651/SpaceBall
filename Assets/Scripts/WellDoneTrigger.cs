using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellDoneTrigger : MonoBehaviour {

    public GameObject canvasObject; // drag your canvas object to this variable in the editor
    public GameObject canvasObject1;
    public GameObject WellDoneCanvas;
    // Use this for initialization
    void OnTriggerEnter(Collider col)
    {
        if (this.gameObject.tag== "WellDoneTrigger" && col.gameObject.tag == "Player")
        {
            canvasObject.SetActive(false);
            canvasObject1.SetActive(false);
            WellDoneCanvas.SetActive(true);


        }
        if (this.gameObject.tag == "NextSceneLoader" && col.gameObject.tag == "Player")
        {

            Application.LoadLevel("TutorialScene6");


        }
    }
}
