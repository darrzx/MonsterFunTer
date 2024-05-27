using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationScript : MonoBehaviour
{
    public Animator notification;
    public TextMeshProUGUI notificationText;
    public static int flag = 0;
    public static bool inOut = false;
    private bool isPlay = false;
    private bool isEnd = false;

    // Update is called once per frame
    void Update()
    {
        if (flag == 0)
        {
            notificationText.SetText("Mission Completed!");
        }
        else
        {
            notificationText.SetText("Cheat Activated!");
        }

        if (inOut)
        {
            notification.SetBool("isEnter", true);
            Invoke("notifEnd", 2f);
        }
        else
        { 
            notification.SetBool("isEnter", false);
        }
    }

    public void notifEnd()
    {
        inOut = false;
    }
}
