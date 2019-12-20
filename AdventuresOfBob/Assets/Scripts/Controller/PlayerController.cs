using RPG.Combat;
using RPG.PlayerMove;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        RaycastHit hit;
        void Update()
        {
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
                if(Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target);
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
