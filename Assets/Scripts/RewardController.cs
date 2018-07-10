using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class RewardController : MonoBehaviour
{
    private GameObject player;
    private ScoreManager scoreManager;

    public bool isSpinning;
    public int rewards = 1;

    private bool isInMagneticField = false;

    public AudioClip clip;
    private AudioSource audSource;

    // Use this for initialization
    void Start()
    {
        foreach(GameObject plr in GameObject.FindGameObjectsWithTag("Player")){
            if(plr.GetComponent<PlayerController>().getisClient()){
                player = plr;
            }
        }

        scoreManager = player.GetComponent<ScoreManager>();

        audSource = GetComponent<AudioSource>();
        audSource.clip = clip;
        audSource.loop = false;
        audSource.volume = 1F;
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
        if(transform.position.y < -10 ){

            Destroy(gameObject); 
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
        if (col.gameObject.tag == "Player")
        {
            scoreManager.AddScore(rewards);
            AudioSource.PlayClipAtPoint(clip,gameObject.transform.position);
            //Debug.Log("In play");
            //Thread.Sleep(5);
            // 0.1f works fine, but then sound is not good
            Destroy(gameObject);
            //gameObject.SetActive(false);
            //GetComponent<MeshRenderer>().enabled = false;

        }

        else if (col.gameObject.tag == "death")
        {
            Destroy(gameObject); 
        }
    }

    // Destroyer
    void OnBecameInvisible()
    {
        //Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "MagneticField")
        {
            isInMagneticField = true;
        }
    }


    void onDestroy(){

    }
}