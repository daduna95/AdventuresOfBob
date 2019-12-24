using RPG.Combat;
using RPG.Controller;
using RPG.Core;
using RPG.PlayerMove;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 5f;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float waypointDwellTime =3f;
    
    Fighter fighter;
    Health health;
    GameObject player;
    PlayerMoveScript mover;


    Vector3 guardPosition;
    float lastTimeGuardSawPlayer = Mathf.Infinity;
    float lastTimeArrivedAtWaypoint = Mathf.Infinity;
    int currentWaypointIndex = 0;


    
    private void Start()
    {
        mover = GetComponent<PlayerMoveScript>();
        fighter = GetComponent<Fighter>();
        health = GetComponent<Health>();
        player = GameObject.FindWithTag("Player");
        guardPosition = transform.position;
    }
    void Update()
    {
        if (health.IsDead()) return;
        if (InAttackRangeToPlayer() && fighter.CanAttack(player))
        {
            AttackBehavour();
        }
        else if (lastTimeGuardSawPlayer < suspicionTime)
        {
            SuspicionBehavour();
        }
        else
        {
            PatrolBehavour();
        }

        UpdateTimers();
    }

    private void UpdateTimers()
    {
        lastTimeGuardSawPlayer += Time.deltaTime;
        lastTimeArrivedAtWaypoint += Time.deltaTime;
    }

    private void PatrolBehavour()
    {
        Vector3 nextWaypoint = guardPosition;

        if(patrolPath != null)
        {
            if(AtWaypoint())
            {
                lastTimeArrivedAtWaypoint = 0;
                CycleWaypoint();
            }
            nextWaypoint = GetCurrentWaypoint();
        }
        if(lastTimeArrivedAtWaypoint > waypointDwellTime)
        {
            mover.StartMoveAction(nextWaypoint);
        }
        
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointTolerance;
    }
    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }
    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWayPoint(currentWaypointIndex);
    }

    private void SuspicionBehavour()
    {
        GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    private void AttackBehavour()
    {
        lastTimeGuardSawPlayer = 0;
        fighter.Attack(player);
    }


    private bool InAttackRangeToPlayer()
    {
        bool distanceToPlayer = Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        return distanceToPlayer;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

}
