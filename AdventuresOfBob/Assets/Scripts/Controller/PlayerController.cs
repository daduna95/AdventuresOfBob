using RPG.Combat;
using RPG.Core;
using RPG.PlayerMove;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
        RaycastHit hit;


        private void Start()
        {
            health = GetComponent<Health>();
        }
        void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            
        }

        private bool InteractWithMovement()
        {
            Ray lastRay = GetMouseRay();
            bool isHit = Physics.Raycast(lastRay, out hit);
            if (isHit == true)
            {
                if(Input.GetMouseButton(0))
                {
                    GetComponent<PlayerMoveScript>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(var hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;


                if(!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }
                if(Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

       

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

    }
}
