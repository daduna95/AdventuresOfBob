using RPG.Core;
using RPG.PlayerMove;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Health target;
        [SerializeField] float range =2f;
        [SerializeField] float timeBetweenAttack = 1f;

        float lastAttackTime = Mathf.Infinity;
        private void Update()
        {
            lastAttackTime += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead()) return;
            if (!GetInRange())
            {
                GetComponent<PlayerMoveScript>().Mover(target.transform.position);
            }
            else
            {
                GetComponent<PlayerMoveScript>().Cancel();
                AttackBehavour();
            }
        }

        private void AttackBehavour()
        {
            transform.LookAt(target.transform);
            if(lastAttackTime > timeBetweenAttack)
            {
                ResetAttack();
                lastAttackTime = 0f;
            }
        }

        private void ResetAttack()
        {
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        public void Hit()
        {
            if (target == null) return;
            target.GetComponent<Health>().TakeDamage(5);
        }

        private bool GetInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < range;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
           
            target = combatTarget.GetComponent<Health>();
        }
        public void Cancel()
        {
            CancelAttack();
            target = null;
        }

        private void CancelAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
        }
    }
}
