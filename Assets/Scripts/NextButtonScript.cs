using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextButtonScript : MonoBehaviour {

	
	public void ChangeScene (string sceneName) {

		Application.LoadLevel(sceneName);
		
	}
	
	
}
