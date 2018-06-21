using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour
{
    public GameObject notificationCanvas;
    public Text notificationText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NotifyText(string text)
    {
        notificationCanvas.SetActive(true);
        notificationText.text = text;

        StartCoroutine(CloseAfterSeconds(3));
    }

    public void NotifyLevelUp(int newLevel)
    {
        notificationCanvas.SetActive(true);

        StartCoroutine(CloseAfterSeconds(3));
    }

    IEnumerator CloseAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        notificationCanvas.SetActive(false); 
    }
}
