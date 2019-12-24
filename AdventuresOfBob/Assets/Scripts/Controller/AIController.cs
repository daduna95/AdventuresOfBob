using RPG.Combat;
using RPG.Core;
using RPG.PlayerMove;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] float chaseDistance = 5f;
    [SerializeField]float suspicionTime = 5f;

    float lastTimeGuardSawPlayer;
    Fighter fighter;
    Health health;
    GameObject player;
    Vector3 guardPosition;
    PlayerMoveScript mover;


    
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
        if(InAttackRangeToPlayer() && fighter.CanAttack(player))
        {
            lastTimeGuardSawPlayer = 0;
            AttackBehavour();
        }
        else if(lastTimeGuardSawPlayer < suspicionTime)
        {
            SuspicionBehavour();
        }
        else
        {
            GuardBehavour();
        }
        lastTimeGuardSawPlayer += Time.deltaTime; 
    }

    private void GuardBehavour()
    {
        mover.StartMoveAction(guardPosition);
    }

    private void SuspicionBehavour()
    {
        GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    private void AttackBehavour()
    {
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
