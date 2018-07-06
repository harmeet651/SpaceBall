using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class missile : MonoBehaviour {

    float missileVelocity = 14;
    Rigidbody homingMissile;
    float turn = 20;
    int fuseDelay = 20;
    GameObject missileMod;
    public float speed;
    public float verticalVelocity = 10f;
    public bool isFlying = false;
    public float altitude=0.5f;

    //ParticleSystem SmokePrefab;
    //AudioClip missileClip;

    //public Transform target;

    void Start()
    {
        //    Fire();
        homingMissile = GetComponent<Rigidbody>();
        //homingMissile.detectCollisions = false;

    }

    void Update()

    {
        if (homingMissile == null)
        {
            Destroy(homingMissile);
            return;
        }

        //homingMissile.velocity = transform.forward * missileVelocity;
        //if (transform.position.y < altitude)
        //{
        //    homingMissile.velocity = new Vector3(0, verticalVelocity, missileVelocity);
        //}
        //else
        {
            homingMissile.velocity = new Vector3(0, 0, missileVelocity);
        }
        //if (isFlying)
        //{
        //    Debug.Log("In flying");
        //    homingMissile.velocity = new Vector3(0, verticalVelocity, missileVelocity * 3);
        //}

        //else
        //{
        //    Debug.Log("In other");
        //    homingMissile.velocity = new Vector3(0, verticalVelocity, missileVelocity);
        //}

        //var targetRotation = Quaternion.LookRotation(target - transform.position);

        //homingMissile.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));

        //Destroy(homingMissile, 4.0f);
    }
    //public void Fly()
    //{
    //    // Actual flying motion is done through missillleee
    //    GetComponent<missillleee>().Fly();
    //}

    //void Fire()

    //{

    //    //Thread.Sleep(fuseDelay);
    //    //AudioSource.PlayClipAtPoint(missileClip, transform.position);

    //    var distance = Mathf.Infinity;

    //    foreach (GameObject go in GameObject.FindGameObjectsWithTag("death"))
    //    {
    //        var diff = (go.transform.position - transform.position).sqrMagnitude;

    //        if (diff < distance)
    //        {
    //            distance = diff;
    //            target = go.transform;
    //        }

    //    }

    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "death")
        {
            Debug.Log("inside destroy missile and object");
            
            Destroy(collision.gameObject);
            
            Destroy(this.gameObject);
            //GetComponent<Rigidbody>().Sleep();
         }
        //else
        //{
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        //    if(other.tag=="missileTrigger")
        //    {
        //        Debug.Log("Destroy needle");
        //        homingMissile.detectCollisions = false;
        //    }
        //    if (other.tag == "missileTriggerDisable")
        //    {
        //        homingMissile.detectCollisions = true;

        //    }
    }

}


    
