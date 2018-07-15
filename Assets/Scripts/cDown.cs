using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cDown : MonoBehaviour {

	public static int state;	
	private float startTime;
	public Sprite count3, count2, count1;
	private Image cdImage; 
	// Use this for initialization
	void Start () {
		cdImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("startTime: " +startTime + " currentTime: "+ Time.realtimeSinceStartup);
		if(state == 0)
		{
			cdImage.sprite = count3;			
			startTime = Time.realtimeSinceStartup;
			state++;
		}
		if(state == 1)
			if(Time.realtimeSinceStartup > (startTime + 0.6f)){
				state++;
				cdImage.sprite = count2;
				//Debug.Log("Changed image");
			}
		if(state == 2)
			if(Time.realtimeSinceStartup > (startTime + 1.2f)){
				cdImage.sprite = count1;
			}
		
	}
}
