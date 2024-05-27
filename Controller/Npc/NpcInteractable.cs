using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcInteractable : MonoBehaviour
{
    public GameObject interactiveText;

    public void Interact()
    {
        interactiveText.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            interactiveText.SetActive(true);
            NpcController.StateInteract();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactiveText.SetActive(false);
        NpcController.StateIdle();
    }
}
