using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MazeCutSceneController : MonoBehaviour
{

    public CinemachineVirtualCamera MazeCamCutScene;
    public GameObject timeline;
    public GameObject PlayerCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MissionController.isMaze)
        {
            timeline.SetActive(true);
            PlayerCanvas.SetActive(false);
            MazeCamCutScene.Priority = 100;
            Invoke("changePriority", 8f);
            MissionController.isMaze = false;
        }
    }

    void changePriority()
    {
        MazeCamCutScene.Priority = 0;
        timeline.SetActive(false);
        PlayerCanvas.SetActive(true);
    }
}
