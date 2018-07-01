using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum FlightStatus
{
    None,
    Launching,
    Flying,
    Landing
}

public class FlightController : MonoBehaviour
{
    public GameObject wing;
    private Vector3 wingOffset;

    Rigidbody rb;
    private float launchSpeed = 20.0f;
    private float flightAltitude = 8.0f;
    private float flightLength = 100.0f;
    private FlightStatus flightStatus = FlightStatus.None;
    private float flightBeginZPos;

    private PlayerController playerController;

    private float flag = 0;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(GetComponent<PlayerController>() != null){
            flag = 1;
            rb = GetComponent<Rigidbody>();
            playerController = GetComponent<PlayerController>();

            wingOffset = wing.transform.position - transform.position;
        }

        if(flag == 1){
            wing.transform.position = transform.position + wingOffset;

            if (flightStatus == FlightStatus.Launching)
            {
                if (transform.position.y < flightAltitude)
                {
                    playerController.verticalVelocity = launchSpeed;
                }

                else
                {
                    playerController.verticalVelocity = 0;
                    flightStatus = FlightStatus.Flying;
                    flightBeginZPos = transform.position.z;
                    playerController.isFlying = true;
                }
            }

            else if (flightStatus == FlightStatus.Flying)
            {
                if (flightBeginZPos + flightLength < transform.position.z)
                {
                    flightStatus = FlightStatus.Landing;
                    rb.useGravity = true;
                }
            }

            else if (flightStatus == FlightStatus.Landing)
            {
                if (transform.position.y < 3.0f)
                {
                    playerController.isFlying = false;
                    playerController.DisableMagneticField();
                    flightStatus = FlightStatus.None;
                    wing.SetActive(false);
                }
            }
        }

    }

    public void Fly()
    {
        rb.useGravity = false;

        flightStatus = FlightStatus.Launching;
        wing.SetActive(true);
    }
}
