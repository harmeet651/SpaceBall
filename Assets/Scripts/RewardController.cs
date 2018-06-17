using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    public bool isSpinning;
    public int rewards;
    public float cubeZAxis=0;
    public GameObject objSwn;

    // Use this for initialization
    void Start()
    {
        cubeZAxis = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(new Vector3(3, 3, 3));
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Player")
        {
                rewards = col.GetComponent<PlayerController>().rewards++;
                Destroy(gameObject);
                if (rewards == 3)
                {
                //Vector3 a = Vector3(transform.position.x - 0.5f, transform.position.y - 1.2f, cubeZAxis - 62);
                    Instantiate(objSwn, transform.position + new Vector3(transform.position.x - 0.5f, transform.position.y - 1.2f, cubeZAxis+1), transform.rotation);
                }
        }
    }
}