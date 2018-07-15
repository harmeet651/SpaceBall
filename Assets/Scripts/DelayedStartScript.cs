using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedStartScript : MonoBehaviour {

	public GameObject countDown;
	public static bool perform = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(perform)
			StartCoroutine ("StartDelay");
	}

	IEnumerator StartDelay(){
		perform = false;
		countDown.gameObject.SetActive (true);
		cDown.state = 0;
		Time.timeScale = 0;
		float pauseTime = Time.realtimeSinceStartup + 1.8f;
		while (Time.realtimeSinceStartup < pauseTime)
			yield return 0;
		countDown.gameObject.SetActive (false);
		Time.timeScale = 1;
	}
}
