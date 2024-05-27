using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{

    private PlayerObjectController instance = PlayerObjectController.getInstance();
    [SerializeField]
    private Animator _animator;
    public static int missionCount;
    public GameObject Lyra, Darian, Cesiya, Portal, portalMenu;
    private string[] missionList = { "Find and talk to Lyra", "INSTANTIATE ATTACK", "USE YOUR SKILL ABILITY", "TALK TO ALL PEOPLE", "Go to Maze" };
    public static int[] missionProgress = { 0, 0, 0, 0 };
    public static bool isLyra = false;
    private bool isDarian = false;
    private bool isCesiya = false;
    private bool isPortal = false;
    private bool interactDarian = false;
    private bool interactCesiya = false;
    public TextMeshProUGUI missiontext;

    public static bool isMaze = false;
    public GameObject loadingScreen;
    public Slider slider;

    public TextMeshProUGUI dialog;
    string[] text = { "Lyra : Oh hi there ! Must be a new heroes here right?",
        "Lyra : Please show me your basic skills 10 times!",
        "Lyra : Great job ! I can see your basic skills. Now, show me your advance skills!",
        "Lyra : Your skills are really beautiful!, Now meet all of my friends here and talk to them!",
        "Lyra : Awesome, now Go to the maze and kill all the monster! Good luck my friends!"};
    string[] textNPC = {"Darian : Help I'm so scared, monster!, you have to defeat all the monsters!",
        "Cesiya : Hi Hero, i need your help to defeat them!"};
    int letterPerSeconds = 20;

    // Start is called before the first frame update
    void Start()
    {
        missiontext.SetText(missionList[0]);
        missionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.C) && missionCount == 0 && isLyra)
            {
                StartCoroutine(dialogTyping(text[0]));
                missiontext.color = Color.green;
                NotificationScript.flag = 0;
                NotificationScript.inOut = true;
                missionCount = 1;
            }
            else if (Input.GetKeyDown(KeyCode.C) && missiontext.color == Color.green && missionCount == 1)
            {
                StartCoroutine(dialogTyping(text[1]));
                Invoke("unactiveText", 4f);
                missiontext.color = Color.white;
                missiontext.SetText(missionList[missionCount] + " (" + missionProgress[0] + "/10)");
            }
            else if (missionProgress[0] != 0 && missionProgress[0] != 10  && missionCount == 1)
            {
                missiontext.SetText(missionList[missionCount] + " (" + missionProgress[0] + "/10)");
            }
            else if(missionProgress[0] == 10 && missionCount == 1)
            {
                missiontext.SetText(missionList[missionCount] + " (10/10)");
                NotificationScript.flag = 0;
                missiontext.color = Color.green;
                NotificationScript.inOut = true;
                missionCount = 2;
            }
            else if(Input.GetKeyDown(KeyCode.C) && missiontext.color == Color.green && missionCount == 2 && isLyra)
            {
                StartCoroutine(dialogTyping(text[2]));
                Invoke("unactiveText", 6f);
                missiontext.color = Color.white;
                missiontext.SetText(missionList[missionCount] + " (0/2)");
            }
            else if(missionProgress[1] != 0 && missionProgress[1] != 2 && missionCount == 2)
            {
                missiontext.SetText(missionList[missionCount] + " (" + missionProgress[1] + "/2)");
            }
            else if(missionProgress[1] == 2 && missionCount == 2)
            {
                missiontext.SetText(missionList[missionCount] + " (2/2)");
                missiontext.color = Color.green;
                NotificationScript.flag = 0;
                NotificationScript.inOut = true;
                missionCount = 3;
            }
            else if (Input.GetKeyDown(KeyCode.C) && missiontext.color == Color.green && missionCount == 3 && isLyra)
            {
                StartCoroutine(dialogTyping(text[3]));
                Invoke("unactiveText", 6f);
                missiontext.color = Color.white;
                missionProgress[2] = 1;
                missiontext.SetText(missionList[missionCount] + " (" + missionProgress[2] + "/3)");
            }
            else if(Input.GetKeyDown(KeyCode.C)&& isDarian && missionCount == 3 && interactDarian == false)
            {
                StartCoroutine(dialogTyping(textNPC[0]));
                Invoke("unactiveText", 6f);
                interactDarian = true;
                missionProgress[2]++;
                missiontext.SetText(missionList[missionCount] + " (" + missionProgress[2] + "/3)");
            }
            else if (Input.GetKeyDown(KeyCode.C) && isCesiya && missionCount == 3 && interactCesiya == false)
            {
                StartCoroutine(dialogTyping(textNPC[1]));
                Invoke("unactiveText", 5f);
                interactCesiya = true;
                missionProgress[2]++;
                missiontext.SetText(missionList[missionCount] + " (" + missionProgress[2] + "/3)");
            }
            else if (missionProgress[2] == 3 && missionCount == 3)
            {
                missiontext.color = Color.green;
                NotificationScript.flag = 0;
                NotificationScript.inOut = true;
                missionCount = 4;
            }
            else if (Input.GetKeyDown(KeyCode.C) && missiontext.color == Color.green && missionCount == 4 && isLyra)
            {
                Invoke("mazeText", 9f);
                missiontext.color = Color.white;
                missiontext.SetText(missionList[missionCount]);
                isMaze = true;
            }
            else if (Input.GetKeyDown(KeyCode.J) && isPortal && missionCount == 4)
            {
                missiontext.color = Color.green;
                NotificationScript.flag = 0;
                NotificationScript.inOut = true;
                Invoke("DestroyMissionText", 4f);
                portalMenu.SetActive(false); 
                missionCount = 5;
                LoadLevel(2);
            }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Lyra)
        {
            isLyra = true;
        }
        else if (other.gameObject == Darian && isDarian == false)
        {
            isDarian = true;
        }
        else if (other.gameObject == Cesiya && isCesiya == false)
        {
            isCesiya = true;
        }
        else if(other.gameObject == Portal)
        {
            isPortal = true;
            if(missionCount == 4)
            {
                portalMenu.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPortal = false;
        isLyra = false;
        portalMenu.SetActive(false);
    }

    public void notifEnd()
    {
        NotificationScript.inOut = false;
    }

    public IEnumerator dialogTyping(string dialogContext)
    {
        dialog.text = "";
        foreach (var letter in dialogContext.ToCharArray())
        {
            dialog.text += letter;
            yield return new WaitForSeconds(1f / letterPerSeconds);
        }
    }

    public void unactiveText()
    {
        dialog.text = "";
    }

    public void mazeText()
    {
        StartCoroutine(dialogTyping(text[4]));
        Invoke("unactiveText", 7f);
    }

    void DestroyMissionText()
    {
        missiontext.enabled = false;
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            yield return null;
        }
    }
}
