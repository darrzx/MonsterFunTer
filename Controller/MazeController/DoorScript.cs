using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animator animator;
    private bool isDoor = false;
    public GameObject DoorScreen, openText, closeText;
    public AudioClip openAudio, closeAudio;
    public AudioSource door;
    public static bool isOpenDoor = true;

    // Start is called before the first frame update
    void Start()
    {
        isOpenDoor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && isDoor && isOpenDoor)
        {
            if (!animator.GetBool("isOpen"))
            {
                door.PlayOneShot(openAudio);
                animator.SetBool("isOpen", true);
            }
            else
            {
                door.PlayOneShot(closeAudio);
                animator.SetBool("isOpen", false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && isOpenDoor)
        {
            DoorScreen.SetActive(true);
            if (!animator.GetBool("isOpen"))
            {
                openText.SetActive(true);
                closeText.SetActive(false);
            }
            else
            {
                closeText.SetActive(true);
                openText.SetActive(false);
            }
            isDoor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DoorScreen.SetActive(false);
        isDoor = false;
        if (animator.GetBool("isOpen"))
        {
            door.PlayOneShot(closeAudio);
            animator.SetBool("isOpen", false);
        }
    }
}
