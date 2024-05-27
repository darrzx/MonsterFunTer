using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinSkill : MonoBehaviour
{
    private PlayerObjectController instance = PlayerObjectController.getInstance();
    public Animator _animator;
    public AudioSource basicAttack;
    public AudioSource rage;
    public AudioSource roll;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;

    float attackSpeed = 0.5f;
    float lastRageTime = 0;
    float lastRollTime = 0;
    public GameObject skill1, skill1Cooldown, skill2, skill2Cooldown;

    private bool isSkill1Used = false;
    private bool isSkill2Used = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
                PaladinAttack.isAttack = true;
                OnClick();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                PaladinAttack.isAttack = false;
            }
        }

        // SET ANIMASI RAGE
        if (Input.GetKeyDown(KeyCode.R) && (Time.time - lastRageTime) > 5f)
        {
            if (MissionController.missionCount == 2 && isSkill1Used == false)
            {
                MissionController.missionProgress[1]++;
                isSkill1Used = true;
            }
            _animator.SetBool("isRage", true);
            instance.setMovementSpeed(0.02f);
            attackSpeed = 0.3f;
            skill1.SetActive(false);
            skill1Cooldown.SetActive(true);
            lastRageTime = Time.time;
            rage.Play();
        }
        else
        {
            _animator.SetBool("isRage", false);
            if (Time.time - lastRageTime > 5f)
            {
                skill1.SetActive(true);
                skill1Cooldown.SetActive(false);
                instance.setMovementSpeed(0.01f);
                attackSpeed = 0.5f;
            }
        }

        // SET ANIMASI ROLL
        if (Input.GetKeyDown(KeyCode.F) && (Time.time - lastRollTime) > 3f)
        {
            if (MissionController.missionCount == 2 && isSkill2Used == false)
            {
                MissionController.missionProgress[1]++;
                isSkill2Used = true;
            }
            _animator.SetBool("isRoll", true);
            StartCoroutine(Roll());
            lastRollTime = Time.time;
            skill2.SetActive(false);
            skill2Cooldown.SetActive(true);
            roll.Play();
        }
        else
        {
            _animator.SetBool("isRoll", false);
            if (Time.time - lastRollTime > 3f)
            {
                skill2.SetActive(true);
                skill2Cooldown.SetActive(false);
            }
        }

        IEnumerator Roll()
        {
            float duration = 0.8f;
            float speed = 1f;
            float distance = 2f;

            Vector3 startPosition = transform.position;
            Vector3 endPosition = transform.position + transform.forward * distance;

            float t = 0f;
            while (t < duration)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, t / duration);
                t += Time.deltaTime * speed;
                yield return null;
            }
            transform.position = endPosition;

            yield return new WaitForSeconds(duration / 2f);
        }
    }

    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        if (noOfClicks == 1)
        {
            _animator.SetBool("isAttacking1", true);
            if (MissionController.missionCount == 1 && MissionController.missionProgress[0] < 10)
            {
                MissionController.missionProgress[0]++;
            }
            basicAttack.Play();
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if (noOfClicks >= 2 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > attackSpeed && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            _animator.SetBool("isAttacking1", false);
            _animator.SetBool("isAttacking2", true);
            basicAttack.Play();
        }
        if (noOfClicks >= 3 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > attackSpeed && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            _animator.SetBool("isAttacking2", false);
            _animator.SetBool("isAttacking3", true);
            basicAttack.Play();
        }
    }
}
