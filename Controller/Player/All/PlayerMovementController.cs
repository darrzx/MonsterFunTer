using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementController : MonoBehaviour
{

    private PlayerObjectController instance = PlayerObjectController.getInstance();
    [SerializeField]
    private Animator _animator;

    public float jumpForce = 3f;
    public AudioSource jump, land, footstep;
    public Rigidbody rb;

    private float? jumpTime, dodgeTime;
    private float jumpPeriod, dodgePeriod;
    private float lastDodge = -100f;
    private bool isLanding = false;

    public Slider staminaSlider, healthSlider;
    private float maxStamina, maxHealth;
    private float staminaDrainRate = 0.1f;
    private float staminaRegenRate = 0.05f;

    private bool canPickUpMeat = false;
    private bool canPickUpPotion = false;
    public static bool canSetUnactiveMeat = true;
    public static bool canSetUnactivePotion = true;
    public GameObject inventoryBar, meatText, potionText;
    public GameObject meatItemFirst, potionItemFirst;
    public GameObject canvas, map;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        jumpPeriod = 0.4f;
        dodgePeriod = 0.8f;
        maxStamina = instance.getMaxStamina();
        maxHealth = instance.getMaxHealth();
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = instance.getCurrStamina();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = instance.getCurrHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_animator.GetBool("isCasting") && !_animator.GetBool("isFlying") && !_animator.GetBool("isDeath"))
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (canvas.activeSelf)
                {
                    canvas.SetActive(false);
                }
                else
                {
                    canvas.SetActive(true);
                }
            }

            float verticalAxis = Input.GetAxis("Vertical");
            float horizontalAxis = Input.GetAxis("Horizontal");

            Vector3 movement = verticalAxis * transform.forward + horizontalAxis * transform.right;

            // SET ANIMASI JALAN
            if (movement.magnitude != 0)
            {
                footstep.enabled = true;
                footstep.pitch = 0.5f;
                transform.position += movement * instance.getMovementSpeed();
                _animator.SetBool("isMoving", true);
            }
            else
            {
                footstep.enabled = false;
                transform.position += movement * instance.getMovementSpeed();
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
            if (Input.GetKey(KeyCode.LeftShift) && movement.magnitude >= 0.1f && staminaSlider.value != 0)
            {
                footstep.pitch = 0.8f;
                transform.position += movement * 0.05f;
                _animator.SetBool("isRunning", true);
                instance.setCurrStamina(instance.getCurrStamina() - staminaDrainRate);
                staminaSlider.value = instance.getCurrStamina();
                //staminaSlider.value = staminaSlider.value - staminaDrainRate;
            }
            else
            {
                transform.position += movement * instance.getMovementSpeed();
                _animator.SetBool("isRunning", false);
                if (instance.getCurrStamina() < 100)
                {
                    instance.setCurrStamina(instance.getCurrStamina() + staminaRegenRate);
                }
                staminaSlider.value = instance.getCurrStamina();
                //staminaSlider.value = staminaSlider.value + staminaRegenRate;
            }

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
                    Invoke("fullHealth", 2f);
                }
            }
            else
            {
                _animator.SetBool("isDrinking", false);
            }

            // SET ANIMASI LOOTING ITEM
            if (Input.GetKeyDown(KeyCode.C) && canPickUpMeat)
            {
                _animator.SetBool("isLooting", true);
                ItemController.meatCount += 1;
                canPickUpMeat = false;
            }

            if (Input.GetKeyDown(KeyCode.C) && canPickUpPotion)
            {
                _animator.SetBool("isLooting", true);     
                ItemController.potionCount += 1;
                canPickUpPotion = false;
            }

            // SET ANIMASI DEATH
            if (instance.getCurrHealth() <= 0 && !_animator.GetBool("isDeath"))
            {
                _animator.SetBool("isDeath", true);
            }

            // LIAT MAP
            if (Input.GetKey(KeyCode.Tab))
            {
                map.SetActive(true);
            }
            else
            {
                map.SetActive(false);
            }

            //handle animation
            _animator.SetFloat("posY", verticalAxis);
            _animator.SetFloat("posX", horizontalAxis);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Meat")
        {
            inventoryBar.SetActive(true);
            meatText.SetActive(true);
            potionText.SetActive(false);
            canPickUpMeat = true;
            Debug.Log(canPickUpMeat);
            if (_animator.GetBool("isLooting"))
            {
                Debug.Log("masuk looting meat");
                Destroy(collision.gameObject);
                inventoryBar.SetActive(false);
                meatText.SetActive(false);
                canPickUpMeat = false;
                Invoke("unActiveMeat", 2f);
            }
        }
        else if (collision.gameObject.tag == "Potion")
        {
            inventoryBar.SetActive(true);
            potionText.SetActive(true);
            meatText.SetActive(false);
            canPickUpPotion = true;
            Debug.Log(canPickUpPotion);
            if (_animator.GetBool("isLooting"))
            {
                Debug.Log("masuk looting Potion");
                Destroy(collision.gameObject);
                inventoryBar.SetActive(false);
                potionText.SetActive(false);
                canPickUpPotion = false;
                Invoke("unActivePotion", 2f);
            }
        }
        if (collision.gameObject.tag == "Floor")
        {
            var parent = collision.gameObject.transform.parent.gameObject;
            var minimap = parent.transform.Find("MiniMapColor").gameObject;
            var minimapUnactive = parent.transform.Find("MiniMapUnactive").gameObject;
            minimap.SetActive(true);
            minimapUnactive.SetActive(false);
            
            if (parent.ToString() == "EnemyRoom(Clone) (UnityEngine.GameObject)")
            {
                DoorScript.isOpenDoor = false;

                var vampire1 = parent.transform.Find("Vampire A Lusth (1)").gameObject;
                var vampire1Canvas = vampire1.transform.Find("Canvas").gameObject;
                var vampire1Slider = vampire1Canvas.transform.Find("Slider").gameObject;
                Slider vampire1SliderComponent = vampire1Slider.GetComponent<Slider>();

                var vampire2 = parent.transform.Find("Vampire A Lusth (2)").gameObject;
                var vampire2Canvas = vampire2.transform.Find("Canvas").gameObject;
                var vampire2Slider = vampire2Canvas.transform.Find("Slider").gameObject;
                Slider vampire2SliderComponent = vampire2Slider.GetComponent<Slider>();

                if (vampire1SliderComponent.value <= 0 && vampire2SliderComponent.value <= 0)
                {
                    DoorScript.isOpenDoor = true;
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        inventoryBar.SetActive(false);
        meatText.SetActive(false);
        potionText.SetActive(false);
        canPickUpPotion = false;
        canPickUpMeat = false;
    }

    //void PickUpMeat()
    //{
    //    if (Input.GetKeyDown(KeyCode.C) && canPickUpMeat)
    //    {
    //        _animator.SetBool("isLooting", true);
    //        inventoryBar.SetActive(false);
    //        meatText.SetActive(false);
    //        Invoke("unActiveMeat", 2f);
    //        //unActiveMeat();
    //        canSetUnactiveMeat = false;
    //        ItemController.meatCount += 1;
    //    }
    //}

    //void PickUpPotion()
    //{
    //    if (Input.GetKeyDown(KeyCode.C) && canPickUpPotion)
    //    {
    //        _animator.SetBool("isLooting", true);
    //        inventoryBar.SetActive(false);
    //        potionText.SetActive(false);
    //        Invoke("unActivePotion", 2f);
    //        //unActivePotion();
    //        canSetUnactivePotion = false;
    //        ItemController.potionCount += 1;
    //    }
    //}

    void unActiveMeat()
    {
        _animator.SetBool("isLooting", false);
    }

    void unActivePotion()
    {
        _animator.SetBool("isLooting", false);
    }

    void fullStamina()
    {
        staminaSlider.value = maxStamina;
    }

    void fullHealth()
    {
        instance.setCurrHealth(instance.getCurrHealth() + 50);
        if(instance.getCurrHealth() > 100)
        {
            instance.setCurrHealth(100);
        }
    }
}
