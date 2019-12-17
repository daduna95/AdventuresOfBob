using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMoveScript : MonoBehaviour
{
    
    private NavMeshAgent navMesh;
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

   
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            GetPlayerMove();
        }
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("ForwardSpeed", speed);

    }


    private void GetPlayerMove()
    {
        RaycastHit hit;
        Ray lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool isHit = Physics.Raycast(lastRay, out hit);
        if(isHit == true)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }
}
