using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaladinMovementController : MonoBehaviour
{

    [SerializeField]
    private Animator _animator;

    public float jumpForce = 3f;
    //public AudioSource jump, footstep, land;
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

    public GameObject meatItemFirst, potionItemFirst;

    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;

    float movementSpeed = 0.01f;
    float attackSpeed = 0.5f;
    float lastRageTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        jumpPeriod = 0.8f;
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

        if (movement.magnitude != 0)
        {
            transform.position += movement * movementSpeed;
            _animator.SetBool("isMoving", true);
        }
        else
        {
            transform.position += movement * movementSpeed;
            _animator.SetBool("isMoving", false);
        }

        if (Input.GetKey(KeyCode.Space) && _animator.GetBool("isGrounded"))
        {
            //jump.Play();
            jumpTime = Time.time;
            _animator.SetBool("isJumping", true);
            _animator.SetBool("isGrounded", false);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Time.time >= jumpTime + jumpPeriod)
        {
            //land.Play();
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isGrounded", true);
            jumpTime = 0f;
        }

        if (Input.GetKey(KeyCode.LeftShift) && movement.magnitude >= 0.1f && slider.value != 0)
        {
            transform.position += movement * 0.03f;
            _animator.SetBool("isRunning", true);
            slider.value = slider.value - staminaDrainRate;
        }
        else
        {
            transform.position += movement * movementSpeed;
            _animator.SetBool("isRunning", false);
            slider.value = slider.value + staminaRegenRate;
        }

        if (Input.GetKey(KeyCode.Space) && _animator.GetBool("isGrounded"))
        {
            //jump.Play();
            jumpTime = Time.time;
            _animator.SetBool("isJumping", true);
            _animator.SetBool("isGrounded", false);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Time.time >= jumpTime + jumpPeriod)
        {
            //land.Play(); 
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isGrounded", true);
            jumpTime = 0f;
        }

        // SET ANIMASI LOOTING ITEM
        PickUpMeat();
        PickUpPotion();

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

        // SET BASIC ATTACK 
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > attackSpeed && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            _animator.SetBool("isAttacking1", false);
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > attackSpeed && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            _animator.SetBool("isAttacking2", false);
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > attackSpeed && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            _animator.SetBool("isAttacking3", false);
            noOfClicks = 0;
        }
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }

        // SET ANIMASI RAGE
        //Skill 1
        if (Input.GetKeyDown(KeyCode.R))
        {
            _animator.SetBool("isRage", true);
            movementSpeed = 0.02f;
            attackSpeed = 0.3f;

            lastRageTime = Time.time;
        }
        else
        {
            _animator.SetBool("isRage", false);
            if (Time.time - lastRageTime > 5f)
            {
                movementSpeed = 0.01f;
                attackSpeed = 0.5f;

            }
        }

        // SET ANIMASI ROLL
        if (Input.GetKeyDown(KeyCode.F))
        {
            _animator.SetBool("isRoll", true);
        }
        else
        {
            _animator.SetBool("isRoll", false);
        }

        //handle animation
        _animator.SetFloat("posY", verticalAxis);
        _animator.SetFloat("posX", horizontalAxis);
    }

    private void OnTriggerEnter(Collider collision)
    {
        inventoryBar.SetActive(true);
        if (collision.CompareTag("Meat"))
        {
            meatText.SetActive(true);
            potionText.SetActive(false);
            canPickUpMeat = true;

        }
        else if (collision.CompareTag("Potion"))
        {
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

    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        if (noOfClicks == 1)
        {
            _animator.SetBool("isAttacking1", true);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if (noOfClicks >= 2 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > attackSpeed && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            _animator.SetBool("isAttacking1", false);
            _animator.SetBool("isAttacking2", true);
        }
        if (noOfClicks >= 3 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > attackSpeed && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            _animator.SetBool("isAttacking2", false);
            _animator.SetBool("isAttacking3", true);
        }
    }
}
