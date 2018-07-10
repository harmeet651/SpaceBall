using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music : MonoBehaviour {
 
	void Awake () {
        GameObject[] abs = GameObject.FindGameObjectsWithTag("music");
        if(abs.Length>1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
	}
}
