using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticFieldController : MonoBehaviour
{
    private GameObject player;

    // Use this for initialization
    void Start()
    {
        foreach(GameObject plr in GameObject.FindGameObjectsWithTag("Player")){
            if(plr.GetComponent<PlayerController>().getisClient()){
                player = plr;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move with player object
        transform.position = player.transform.position;
    }
}
