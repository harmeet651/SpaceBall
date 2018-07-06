using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MissileStatus
{
    None,
    Launching,
    Flying,
    Landing
}

public class missillleee : MonoBehaviour
{
    public GameObject wing;
    //private Vector3 wingOffset;

    Rigidbody rb;
    private float launchSpeed = 7.0f;
    private float flightAltitude = 7.0f;
    private float flightLength = 10.0f;
    private MissileStatus flightStatus = MissileStatus.None;
    private float flightBeginZPos;

    private missile playerController;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<missile>();

        //wingOffset = wing.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //wing.transform.position = transform.position + wingOffset;

        if (flightStatus == MissileStatus.Launching)
        {
            if (transform.position.y < flightAltitude)
            {
                Debug.Log("launch");
                playerController.verticalVelocity = launchSpeed;
            }

            else
            {
                Debug.Log("isFlying");
                playerController.verticalVelocity = 0;
                flightStatus = MissileStatus.Flying;
                flightBeginZPos = transform.position.z;
                playerController.isFlying = true;
            }
        }

        else if (flightStatus == MissileStatus.Flying)
        {
            if (flightBeginZPos + flightLength < transform.position.z)
            {
                flightStatus = MissileStatus.Landing;
                rb.useGravity = true;
            }
        }

        else if (flightStatus == MissileStatus.Landing)
        {
            if (transform.position.y < 3.0f)
            {
                playerController.isFlying = false;
                //playerController.DisableMagneticField();
                flightStatus = MissileStatus.None;
                wing.SetActive(false);
            }
        }

    }

    public void Fly()
    {
        rb.useGravity = false;

        flightStatus = MissileStatus.Launching;
        //wing.SetActive(true);
    }
}
