using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class missile : MonoBehaviour {

    float missileVelocity = 14;
    Rigidbody homingMissile;
    Vector3 m_EulerAngleVelocity;
    //float turn = 20;
    //int fuseDelay = 20;
    //GameObject missileMod;
    //public float speed;
    //public Transform target;

    void Start()
    {
        homingMissile = GetComponent<Rigidbody>();
        //Set the axis the Rigidbody rotates in (100 in the z axis)
        m_EulerAngleVelocity = new Vector3(0, 0, 1000);
    }

    void Update()

    {
        //once destroyed, we cannot access rigidbody, so return
        if (homingMissile == null)
        {
            Destroy(homingMissile);
            return;
        }
                homingMissile.velocity = transform.forward * missileVelocity;
                //homingMissile.velocity = new Vector3(0, 0, missileVelocity);
                //homingMissile.AddForce(0, 10, 0);
    }

    //for missile rotation
    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        homingMissile.MoveRotation(homingMissile.rotation * deltaRotation);
    }

    //destroy death trap and missile
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "death")
        {
            Debug.Log("inside destroy missile and object");         
            Destroy(collision.gameObject);           
            Destroy(this.gameObject);
         }
    }
    
}


    
