using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class WinController : MonoBehaviour
{
    private PlayerObjectController instance = PlayerObjectController.getInstance();
    public GameObject WinScreenUI;
    public CinemachineBrain brain;
    public static bool isWin = false;

    // Update is called once per frame
    void Update()
    {
        if (isWin)
        {
            brain.enabled = false;
            Invoke("End", 3f);
        }
    }

    public void End()
    {
        Cursor.lockState = CursorLockMode.None;
        brain.enabled = false;
        WinScreenUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void BackToVillage()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
