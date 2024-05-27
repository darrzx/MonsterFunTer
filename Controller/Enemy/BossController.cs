using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class BossController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed;
    public float walkRadius;
    public Animator animator;
    public BoxCollider dmgRightBox, dmgLeftBox, dmgBotBox;

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
    //public GameObject fireskill, particle;
    private string str = null;
    int item = 0;

    public GameObject DeathScreenUI;
    public CinemachineBrain brain;
    public GameObject icon;
    public GameObject fireEffects, particle;

    private void Start()
    {
        isAttack = false;
        currentState = EnemyState.Idle;
        dmgRightBox.enabled = false;
        dmgLeftBox.enabled = false;
        dmgBotBox.enabled = false;
        currHealth = 600;
    }

    private void Update()
    {
        dmgRightBox.enabled = false;
        dmgLeftBox.enabled = false;
        dmgBotBox.enabled = false;
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
                if (distance <= 2f)
                {
                    animator.SetBool("isWalking", false);
                    currentState = EnemyState.Attacking;
                }
                break;
            case EnemyState.Attacking:
                if (!isAttack)
                {
                    isAttack = true;
                    var x = rand.Next(4) + 1;
                    if(x == 1)
                    {
                        dmgLeftBox.enabled = true;
                    }
                    else if(x == 2)
                    {
                        dmgRightBox.enabled = true;
                    }
                    else if(x == 3)
                    {
                        dmgBotBox.enabled = true;
                    }
                    else if(x == 4)
                    {
                        StartCoroutine(Cast());
                    }
                    str = "Attack" + x;
                    animator.SetBool(str, true);
                    Invoke("AttackFalse", 1f);
                }
                break;
            case EnemyState.Die:
                animator.SetBool("isDeath", true);
                break;
        }
    }

    IEnumerator Cast()
    {
        yield return new WaitForSeconds(1.2f);
        fireEffects.SetActive(true);
        particle.SetActive(true);
        yield return new WaitForSeconds(3f);
        animator.SetBool("Attack4", false);
        yield return new WaitForSeconds(0.3f);
        fireEffects.SetActive(false);
        particle.SetActive(false);
        yield return new WaitForSeconds(5f);
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
