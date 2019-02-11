using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public float lookRadius = 10f;

    Transform target;
    NavMeshAgent agent;
    CharacterCombat combat;
    EnemyStats stats;
    CharacterStats targetStats;

    public enum Status { Wander, Combat, LeaveCombat, Dead }
    public Status status;

    public float wanderSpeed = 1;
    public float combatSpeed = 3;
    public bool agentLock;
    float distance;
    float currentWanderTime = 0;

    float wanderTimeMin = 3;
    float wanderTimeMax = 8;
    float wanderRange;
    Vector3 spawnPoint = Vector3.zero;
    Vector3 wanderPoint = Vector3.zero;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<CharacterCombat>();
        stats = GetComponent<EnemyStats>();
        
    }

    void Start () {
        status = Status.Wander;
        target = PlayerManager.instance.player.gameObject.transform;
        targetStats = target.GetComponent<CharacterStats>();
    }
	
	void Update () {
        if (stats.currentHealth == 0)
        {
            status = Status.Dead;
        }
	}

    void LateUpdate()
    {
        switch (status)
        {
            case Status.Wander:
                agent.speed = wanderSpeed;
                agent.stoppingDistance = 0;
                Wander();
                break;
            case Status.Combat:
                agent.speed = combatSpeed;
                agent.stoppingDistance = 2;
                Combat();
                break;
            case Status.LeaveCombat:
                agent.stoppingDistance = 0;
                LeaveCombat();
                break;
            case Status.Dead:
                break;
        }
    }

    void Wander()
    {
        if (currentWanderTime < 0)
        {
            currentWanderTime = Random.Range(wanderTimeMin , wanderTimeMax);
            wanderPoint = spawnPoint + Random.insideUnitSphere * wanderRange;

        }
        else if(agent.remainingDistance == 0)
        {
            currentWanderTime -= Time.deltaTime;
        }

        agent.SetDestination(wanderPoint);

        distance = Vector3.Distance(target.position, transform.position);
        if (distance < lookRadius && targetStats.currentHealth != 0)
        {
            combat.Target(target);
            status = Status.Combat;
        }
    }

    void Combat()
    {
        if(!agentLock)
        agent.SetDestination(target.position);

        if (distance < agent.stoppingDistance)
        {
            FaceTarget();
        }

        if (distance > lookRadius || combat.targetStats == null)
        {
            status = Status.LeaveCombat;
        }
    }

    void LeaveCombat()
    {
        agent.SetDestination(spawnPoint);

        if(agent.remainingDistance < 0.1f && agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            status = Status.Wander;
            stats.ResetStats();
        }
    }

    void Dead()
    {

    }

    public void Respawn(SpawnPoint sp)
    {
        spawnPoint = sp.transform.position;
        wanderRange = sp.wanderRange;

        transform.position = spawnPoint;
        status = Status.Wander;

        stats.ResetStats();

        currentWanderTime = -1;
        GetComponentInChildren<Animator>().SetBool("Death", false);
    }


    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
