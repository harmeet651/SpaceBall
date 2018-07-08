using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class RewardController : MonoBehaviour
{
    private GameObject player;
    private ScoreManager scoreManager;

    public bool isSpinning;
    public int rewards;

    private bool isInMagneticField = false;

    public AudioClip clip;
    private AudioSource audSource;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        scoreManager = player.GetComponent<ScoreManager>();

      //  audSource = GetComponent<AudioSource>();
        //audSource.clip = clip;
        //audSource.loop = false;
        //audSource.volume = 1F;
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
            AudioSource.PlayClipAtPoint(clip,gameObject.transform.position);
            Destroy(gameObject);

        }

        else if (col.gameObject.tag == "death")
        {
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