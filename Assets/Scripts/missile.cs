using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class missile : MonoBehaviour
{

    float missileVelocity = 14;
    Rigidbody homingMissile;
    Vector3 m_EulerAngleVelocity;
    public GameObject Explosion;

    void Start()
    {
        homingMissile = GetComponent<Rigidbody>();
        //Set the axis the Rigidbody rotates in (100 in the z axis)
        m_EulerAngleVelocity = new Vector3(0, 0, 1000);
    }

    void Update()

    {
        // Once destroyed, we cannot access rigidbody, so return
        if (homingMissile == null)
        {
            Destroy(homingMissile);
            return;
        }
        homingMissile.velocity = transform.forward * missileVelocity;
    }

    // Rotate missile
    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        homingMissile.MoveRotation(homingMissile.rotation * deltaRotation);
    }

    // Destroy death trap and missile
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "death")
        {
            Vector3 tempObj;
            GameObject Expl = Explosion;
            Debug.Log("inside destroy missile and object");


            tempObj = collision.gameObject.transform.position;
            tempObj.y = 5;

            // Use explosion after missile bombing here 
            Destroy(collision.gameObject);
            Expl = Instantiate(Expl, tempObj, Quaternion.identity) as GameObject;
            Destroy(this.gameObject);
        }
    }

}



