using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    private PlayerObjectController instance = PlayerObjectController.getInstance();
    public GameObject p, k;
    private GameObject active;
    public CinemachineFreeLook freeLookCam;
    public CinemachineVirtualCamera virtualCam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 rsp = new Vector3(509, 0, 334);

        if(instance.getIndex() == 0)
        {
            k.SetActive(true);
            p.SetActive(false);
            active = k;
        }
        else
        {
            p.SetActive(true);
            k.SetActive(false);
            active = p;
        }

        instance.playerActive = active;
        freeLookCam.LookAt = active.transform;
        freeLookCam.Follow = active.transform;

        //freeLookCam.LookAt = active.transform;
        virtualCam.Follow = active.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
