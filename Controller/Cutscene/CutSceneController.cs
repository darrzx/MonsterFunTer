using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutSceneController : MonoBehaviour
{
    public CinemachineVirtualCamera camCutScene;
    public GameObject PlayerCanvas;

    // Start is called before the first frame update
    void Start()
    {
        camCutScene.Priority = 100;
        Invoke("changePriority", 12f);
        PlayerCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void changePriority()
    {
        camCutScene.Priority = 0;
        PlayerCanvas.SetActive(true);
    }
}
