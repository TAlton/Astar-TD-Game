using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIState : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask WhatisGround, WhatisPlayer;

    //Patrolling State
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking State
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSight, playerInAttackRange;


    private void Moving()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Checking to see if player is within visible range of enemy
        playerInSight = Physics.CheckSphere(transform.position, sightRange, WhatisPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatisPlayer);

        //if player is not in sight range or attack range patrol
        if (!playerInSight && !playerInAttackRange) Patrolling();
        //if player is within sight range but not attack range chase player
        if (playerInSight && !playerInAttackRange) Chasing();
        //if player is within sight and attack range attack player
        if (playerInSight && playerInAttackRange) Attacking();

    }

    private void Patrolling() {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint has been reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint() {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, WhatisGround))
            walkPointSet = true;
    }

    private void Chasing(){

        agent.SetDestination(player.position);
    }

    private void Attacking(){
        //the Attack Code would be here!, just for test purposes so not included

        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack() {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
  
}
