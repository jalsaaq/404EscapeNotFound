using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTarget;
    private Animator animator;

    [Header("Distances")]
    public float attackDistance = 2.5f;
    public float chaseDistance = 15f;

    private string currentState = "Idle";

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }
    }

    void Update()
    {
        if (GameManager.instance == null || playerTarget == null) return;

        int activeSwitches = GameManager.instance.activatedRealGenerators;
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        if (activeSwitches == 0)
        {
            agent.speed = 0f;
            return;
        }

        if (distanceToPlayer <= attackDistance)
        {
            agent.speed = 0f;
            ChangeState("Attack");
        }
        else if (distanceToPlayer <= chaseDistance)
        {
            agent.speed = 6f + activeSwitches;
            ChangeState("Run");
            agent.SetDestination(playerTarget.position);
        }
        else
        {
            agent.speed = 3.5f;
            ChangeState("Walk");
            agent.SetDestination(playerTarget.position);
        }
    }

    void ChangeState(string newState)
    {
        if (animator == null || currentState == newState) return;

        animator.SetTrigger(newState);
        currentState = newState;
    }
}