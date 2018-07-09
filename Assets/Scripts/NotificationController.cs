using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour
{
    public Canvas notificationCanvas;
    public GameObject WellDoneCanvas;
    public Image backgroundImage; 
    public Text notificationText;

    private Color gray = new Color32(55, 55, 55, 80);
    private Color green = new Color32(145, 255, 0, 80);
    private Color red = new Color32(255, 40, 0, 80); 

    // Use this for initialization
    void Start()
    {
        WellDoneCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NotifyText(string text)
    {
        notificationCanvas.enabled = true;
        backgroundImage.color = gray; 
        notificationText.text = text;

        StartCoroutine(CloseAfterSeconds(3));
    }

    public void NotifyHealthChange(int change)
    {
        if (change > 0)
        {
            notificationCanvas.enabled = true; 
            backgroundImage.color = green; 
            notificationText.text = "Health +" + change;

            StartCoroutine(CloseAfterSeconds(0.5f)); 
        }

        else if (change < 0)
        {
            notificationCanvas.enabled = true;
            backgroundImage.color = red;
            notificationText.text = "Health -" + (-change);

            StartCoroutine(CloseAfterSeconds(0.2f));
        }
    }

    IEnumerator CloseAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        notificationCanvas.enabled = false;
    }
}
