using RPG.Core;
using RPG.PlayerMove;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Transform target;
        [SerializeField] float range =2f;
        [SerializeField] float timeBetweenAttack = 1f;

        float lastAttackTime = 0f;
        private void Update()
        {
            lastAttackTime += Time.deltaTime;
            if (target == null) return;
            if (!GetInRange())
            {
                GetComponent<PlayerMoveScript>().Mover(target.position);
            }
            else
            {
                GetComponent<PlayerMoveScript>().Cancel();
                AttackBehavour();
            }
        }

        private void AttackBehavour()
        {
            if(lastAttackTime > timeBetweenAttack)
            {
                GetComponent<Animator>().SetTrigger("Attack");
                lastAttackTime = 0f;
            }
        }

        private bool GetInRange()
        {
            return Vector3.Distance(transform.position, target.position) < range;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
           
            target = combatTarget.transform;
        }
        public void Cancel()
        {
            target = null;
        }

        void Hit()
        {

        }
    }
}
