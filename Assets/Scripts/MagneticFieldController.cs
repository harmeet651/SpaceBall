using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticFieldController : MonoBehaviour {
    private GameObject player;
    private Material mat;
    private float alphaPingPongSpeed = 5.0f;
    private float sizePingPongSpeed = 10.0f;
    Color originalColor;
    public Vector3 originalScale; 
    
    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player");

        Renderer rend = GetComponent<Renderer>();
        mat = rend.material;
        originalColor = mat.GetColor("_Color");
        originalScale = transform.localScale; 
    }
	
	// Update is called once per frame
	void Update () {
        // Move with player object
        transform.position = player.transform.position;

        Color currentColor = originalColor; 
        currentColor.a = (Mathf.PingPong(Time.time * 10.0f, 4) / 30);

        transform.localScale = originalScale * (0.96f + (Mathf.PingPong(Time.time * 10.0f, 8) / 100));

        mat.SetColor("_Color", currentColor); 
	}
}
