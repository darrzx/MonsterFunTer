using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVillage : MonoBehaviour
{

    public AudioSource village;
    public AudioSource outterVillage;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            village.enabled = true;
            outterVillage.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        village.enabled = false;
        outterVillage.enabled = true;
    }
}
