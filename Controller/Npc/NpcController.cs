using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed;
    public float idleTime = 2f;
    public float walkRadius;
    public Animator animator;

    private PlayerObjectController instance = PlayerObjectController.getInstance();
    private enum NpcState { Idle, Moving, Interact };
    private static NpcState currentState = NpcState.Idle;
    private float idleTimer;
    private float interactDistance = 3f;
    private float randomNumber = 0f;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("isWalking", false);
        randomNumber = Random.Range(20, 61);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case NpcState.Idle:
                animator.SetBool("isWalking", false);
                //StartCoroutine(MoveTrigger());
                Invoke("idleNPC", randomNumber);
                break;

            case NpcState.Moving:
                animator.SetBool("isWalking", true);
                SetDestination();
                //Invoke("IfDestinationArrived", 5f);
                break;

            case NpcState.Interact: 
                agent.SetDestination(transform.position);
                animator.SetBool("isWalking", false);
                var lookPos = instance.playerActive.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
                break;
        }
    }
    public void SetDestination(Vector3? target = null)
    {
        if (target == null)
        {
            Vector3 randomPosition = Random.insideUnitSphere * walkRadius;
            NavMesh.SamplePosition(randomPosition + transform.position, out NavMeshHit hit, walkRadius, 1);
            agent.SetDestination(hit.position);
            currentState = NpcState.Moving;
        }
    }

    public Vector3 RandomMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * walkRadius;

        randomPosition += transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    //private IEnumerator MoveTrigger()
    //{
    //    while (currentState == NpcState.Idle)
    //    {
    //        yield return new WaitForSeconds(4f);
    //        currentState = NpcState.Moving;
    //    }
    //}

    public void idleNPC()
    {
        while (currentState == NpcState.Idle)
        {
            currentState = NpcState.Moving;
        }
    }

    public static void StateInteract()
    {
        currentState = NpcState.Interact;
    }

    public static void StateIdle()
    {
        currentState = NpcState.Idle;
    }

    public void IfDestinationArrived()
    {
        animator.SetBool("isWalking", false);
        StateIdle();
    }
}
