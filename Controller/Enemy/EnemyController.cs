using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed;
    public float walkRadius;
    public Animator animator;
    public BoxCollider dmgBox;

    private PlayerObjectController instance = PlayerObjectController.getInstance();
    public enum EnemyState { Idle, Chase, Attacking, Die };
    private EnemyState currentState = EnemyState.Idle;
    public static bool isAttack = false;
    private bool isDeath = false;
    private bool isChance = false;
    public Slider healthSlider;
    private float currHealth;
    private int ctr = 0;

    private System.Random rand = new System.Random();

    private string str = null;
    public GameObject meat, potion;
    int item = 0;

    public GameObject DeathScreenUI;
    public CinemachineBrain brain;
    public GameObject icon;

    private void Start()
    {
        isAttack = false;
        currentState = EnemyState.Idle;
        dmgBox.enabled = false;
        currHealth = 30;
        meat.SetActive(false);
        potion.SetActive(false);
    }

    private void Update()
    {
        dmgBox.enabled = false;
        if (healthSlider.value <= 0)
        {
            currentState = EnemyState.Die;
            icon.SetActive(false);
        }
        if (instance.getCurrHealth() <= 0)
        {
            brain.enabled = false;
            currentState = EnemyState.Idle;
            Invoke("End", 5f);
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                animator.SetBool("isWalking", false);
                if (str != null)
                    animator.SetBool(str, false);
                break;
            case EnemyState.Chase:
                animator.SetBool("isWalking", true);
                agent.SetDestination(instance.playerActive.transform.position);
                var distance = Vector3.Distance(transform.position, instance.playerActive.transform.position);
                if (distance <= 1.6f)
                {
                    animator.SetBool("isWalking", false);
                    currentState = EnemyState.Attacking;
                }
                break;
            case EnemyState.Attacking:
                dmgBox.enabled = true;
                if (!isAttack)
                {
                    isAttack = true;
                    var x = rand.Next(3) + 1;
                    str = "Attack" + x;
                    animator.SetBool(str, true);
                    Invoke("AttackFalse", 1f);
                }
                break;
            case EnemyState.Die:
                animator.SetBool("isDeath", true);
                var chance = randomChance();
                if(chance == 1)
                {
                    item = randomItem();
                    if (item == 1)
                    {
                        meat.SetActive(true);
                    }
                    else if (item == 2)
                    {
                        potion.SetActive(true);
                    }
                }
                
                break;
        }
    }

    public int randomChance()
    {
        var chance = 0;
        if (isChance == false)
        {
            chance = rand.Next(2) + 1;
            isChance = true;
        }
        return chance;
    }

    public int randomItem()
    {
        var item = 0;
        if(isDeath == false)
        {
            PlayerMovementController.canSetUnactiveMeat = true;
            PlayerMovementController.canSetUnactivePotion = true;
            item = rand.Next(2) + 1;
            isDeath = true;
        }
        return item;
    }

    private void AttackFalse()
    {
        animator.SetBool(str, false);
        isAttack = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            currentState = EnemyState.Chase;
            agent.isStopped = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            currentState = EnemyState.Idle;
            agent.isStopped = true;
        }
    }

    public void setCurrHealth(float damage)
    {
        currHealth -= damage;
        healthSlider.value = currHealth;
    }

    public void End()
    {
        Cursor.lockState = CursorLockMode.None;
        brain.enabled = false;
        DeathScreenUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}