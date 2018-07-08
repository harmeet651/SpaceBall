using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    private GameObject player;
    public int temper;
    private Material mat;
    private float opacityPingPongSpeed = 10.0f;
    private float sizePingPongSpeed = 10.0f;
    Color originalColor;
    public Vector3 originalScale;
    public GameObject Expl1;
    

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        Renderer rend = GetComponent<Renderer>();
        mat = rend.material;
        originalColor = mat.GetColor("_Color");
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Move with player object
        transform.position = player.transform.position;

        Color currentColor = originalColor;
        currentColor.a = (Mathf.PingPong(Time.time * opacityPingPongSpeed, 4) / 30);

        transform.localScale = originalScale * (0.96f + (Mathf.PingPong(Time.time * sizePingPongSpeed, 8) / 100));

        mat.SetColor("_Color", currentColor);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "death")
        {
            Vector3 tempObj;

            //Expl.SetActive(true);
            tempObj = col.gameObject.transform.position;
            //use explosion in shield mode here
            Destroy(col.gameObject);
            GameObject Expl=Expl1;
            Expl = Instantiate(Expl, tempObj, Quaternion.identity) as GameObject;

            Debug.Log("in shield death");

        }
    }


}
