using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WizardMovementController : MonoBehaviour
{

    [SerializeField]
    private Animator _animator; 

    public float jumpForce = 3f;
    public AudioSource jump, land;
    public GameObject p;
    public Rigidbody rb;

    private float? jumpTime, dodgeTime;
    private float jumpPeriod, dodgePeriod;
    private float lastDodge = -100f;

    public Slider slider;
    public float maxStamina = 100f;
    private float currentStamina;
    private float staminaDrainRate = 0.05f; // Kecepatan pengurangan stamina saat berlari
    private float staminaRegenRate = 0.05f; // kecepatan pemulihan stamina saat tidak berlari

    public GameObject itemMeat, itemPotion;
    private bool canPickUpMeat = false;
    private bool canPickUpPotion = false;
    private bool canSetUnactiveMeat = true;
    private bool canSetUnactivePotion = true;
    public GameObject inventoryBar, meatText, potionText;
    private bool isLanding = false;

    public GameObject meatItemFirst, potionItemFirst;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        jumpPeriod = 0.4f;
        dodgePeriod = 0.8f;
        currentStamina = maxStamina;
        slider.maxValue = maxStamina;
        slider.value = currentStamina;
    }

    // Update is called once per frame
    void Update()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        Vector3 movement = verticalAxis * transform.forward + horizontalAxis * transform.right;

        // SET ANIMASI JALAN
        if (movement.magnitude != 0)
        {
            transform.position += movement * 0.01f;
            _animator.SetBool("isMoving", true);
        }
        else
        {
            transform.position += movement * 0.01f;
            _animator.SetBool("isMoving", false);
        }

        // SET ANIMASI LOMPAT
        if (Input.GetKey(KeyCode.Space) && _animator.GetBool("isGrounded"))
        {
            jump.Play();
            isLanding = true;
            jumpTime = Time.time;
            _animator.SetBool("isJumping", true);
            _animator.SetBool("isGrounded", false);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Time.time >= jumpTime + jumpPeriod)
        {
            if (isLanding)
            {
                land.Play();
                isLanding = false;
            }
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isGrounded", true);
            jumpTime = 0f;
        }

        // SET ANIMASI LARI
        if (Input.GetKey(KeyCode.LeftShift) && movement.magnitude >= 0.1f && slider.value != 0)
        {
            transform.position += movement * 0.03f;
            _animator.SetBool("isRunning", true);
            slider.value = slider.value - staminaDrainRate;
        }
        else
        {
            transform.position += movement * 0.01f;
            _animator.SetBool("isRunning", false);
            slider.value = slider.value + staminaRegenRate;
        }

        // SET ANIMASI LOOTING ITEM
        PickUpMeat();
        PickUpPotion();

        // SET ANIMASI DODGE
        if (Input.GetKeyDown(KeyCode.V))
        {
            _animator.SetBool("isDodge", true);
            StartCoroutine(Dodge());
        }
        else
        {
            _animator.SetBool("isDodge", false);
        }

        IEnumerator Dodge()
        {
            float duration = 0.8f;
            float speed = 1f;
            float distance = 2f;

            Vector3 startPosition = transform.position;
            Vector3 endPosition = transform.position - transform.forward * distance;

            float t = 0f;
            while (t < duration)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, t / duration);
                t += Time.deltaTime * speed;
                yield return null;
            }
            transform.position = endPosition;

            yield return new WaitForSeconds(duration / 2f);
            lastDodge = Time.time;
        }

        // SET ANIMASI DRINKING
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (meatItemFirst.activeSelf && ItemController.meatCount != 0)
            {
                ItemController.meatCount -= 1;
                _animator.SetBool("isDrinking", true);
                Invoke("fullStamina", 2f);
            }
            else if (potionItemFirst.activeSelf && ItemController.potionCount != 0)
            {
                ItemController.potionCount -= 1;
                _animator.SetBool("isDrinking", true);
                // masukin logic health bar ditambah 50 point
            }
        }
        else
        {
            _animator.SetBool("isDrinking", false);
        }

        //handle animation
        _animator.SetFloat("posY", verticalAxis);
        _animator.SetFloat("posX", horizontalAxis);
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Meat"))
        {
            inventoryBar.SetActive(true);
            meatText.SetActive(true);
            potionText.SetActive(false);
            canPickUpMeat = true;

        }
        else if (collision.CompareTag("Potion"))
        {
            inventoryBar.SetActive(true);
            potionText.SetActive(true);
            meatText.SetActive(false);
            canPickUpPotion = true;
        }
        else
        {
            canPickUpPotion = false;
            canPickUpMeat = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        inventoryBar.SetActive(false);
        meatText.SetActive(false);
        potionText.SetActive(false);
    }

    void PickUpMeat()
    {
        if (Input.GetKeyDown(KeyCode.C) && canPickUpMeat && canSetUnactiveMeat)
        {
            inventoryBar.SetActive(false);
            meatText.SetActive(false);
            unActiveMeat();
            canSetUnactiveMeat = false;
            ItemController.meatCount += 1;

        }
        else
        {
            
        }
    }

    void PickUpPotion()
    {
        if (Input.GetKeyDown(KeyCode.C) && canPickUpPotion && canSetUnactivePotion)
        {
            inventoryBar.SetActive(false);
            potionText.SetActive(false);
            unActivePotion();
            canSetUnactivePotion = false;
            ItemController.potionCount += 1;
        }
        else
        {

        }
    }

    void unActiveMeat()
    {
        Debug.Log("Item Meat diambil!");
        itemMeat.SetActive(false);
    }

    void unActivePotion()
    {
        Debug.Log("Item Potion diambil!");
        itemPotion.SetActive(false);
    }

    void fullStamina()
    {
        slider.value = maxStamina;
    }
}
