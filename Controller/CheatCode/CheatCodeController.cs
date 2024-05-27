using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodeController : MonoBehaviour
{

    private string[] cheatCode = { "hesoyan", "ihateyou", "iloveyou", "icanfly", "akusayangangkatan221" };
    private string input = "";
    private PlayerObjectController instance = PlayerObjectController.getInstance();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (input.ToLower() == cheatCode[0].ToLower())
            {
                instance.setCurrHealth(100);
                instance.setCurrStamina(100);
                NotificationScript.flag = 1;
                NotificationScript.inOut = true;
                Debug.Log("stamina and health full");
                input = "";
            }
            else if (input.ToLower() == cheatCode[1].ToLower())
            {
                instance.setMovementSpeed(0.02f);
                NotificationScript.flag = 1;
                NotificationScript.inOut = true;
                Debug.Log("increase player movement speed");
                input = "";
            }
            else if (input.ToLower() == cheatCode[2].ToLower())
            {
                instance.setCurrHealth(0);
                NotificationScript.flag = 1;
                NotificationScript.inOut = true;
                Debug.Log("player die");
                input = "";
            }
            else if (input.ToLower() == cheatCode[3].ToLower())
            {
                WizardSkill.isCheat = true;
                NotificationScript.flag = 1;
                NotificationScript.inOut = true;
                Debug.Log("add duration and speed flying");
                input = "";
            }
            else if (input.ToLower() == cheatCode[4].ToLower())
            {
                WinController.isWin = true;
                NotificationScript.flag = 1;
                NotificationScript.inOut = true;
                Debug.Log("player wins the game");
                input = "";
            }
            else
            {
                input = "";
            }
        }
        else
        {
            input += Input.inputString;
        }
    }
}