using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTarget;
    private Animator animator;

    // How close the monster needs to be to throw a punch
    public float attackDistance = 2.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }
    }

    void Update()
    {
        // If the GameManager is broken or we can't find the player, don't crash. Just wait.
        if (GameManager.instance == null || playerTarget == null) return;

        // 1. Calculate the distance for the attack
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer <= attackDistance)
        {
            agent.speed = 0f;
            animator.SetTrigger("Attack");
            return;
        }

        // 2. THE REAL CHASE LOGIC (No more hardcoding!)
        int activeSwitches = GameManager.instance.activatedRealGenerators;

        if (activeSwitches == 0)
        {
            agent.speed = 0f;
        }
        else if (activeSwitches == 1)
        {
            agent.speed = 3.5f;
            animator.SetTrigger("Walk");
        }
        else if (activeSwitches >= 2)
        {
            agent.speed = 6f + activeSwitches;
            animator.SetTrigger("Run");
        }

        // 3. Move the monster
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(playerTarget.position);
        }
    }
}