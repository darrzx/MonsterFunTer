using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
public class DeathController : MonoBehaviour
{

    private PlayerObjectController instance = PlayerObjectController.getInstance();
    public GameObject DeathScreenUI;
    public CinemachineBrain brain;

    // Update is called once per frame
    void Update()
    {
        if (instance.getCurrHealth() <= 0)
        {
            brain.enabled = false;
            //EnemyController.currentState = EnemyController.EnemyState.Idle;
            Invoke("End", 5f);
        }
    }

    public void End()
    {
        Cursor.lockState = CursorLockMode.None;
        brain.enabled = false;
        DeathScreenUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
