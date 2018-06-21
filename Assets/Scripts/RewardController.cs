using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    private GameObject player;
    private ScoreManager scoreManager; 

    public bool isSpinning;
    public int rewards;

    private bool isInMagneticField = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        scoreManager = player.GetComponent<ScoreManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(new Vector3(3, 3, 3));
        }

        if (isInMagneticField)
        {
            float speed = player.GetComponent<Rigidbody>().velocity.z * 2;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
    }

    void LateUpdate()
    {
        if (transform.position.y <= -10.0f)
        {
            Destroy(gameObject); 
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player")
        {
            scoreManager.AddScore(rewards);
            Destroy(gameObject); 
            
        }
    }

    // Destroyer
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "MagneticField")
        {
            isInMagneticField = true;
        }
    }
}