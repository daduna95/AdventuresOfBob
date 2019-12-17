using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraScript : MonoBehaviour
{
    [SerializeField] Transform Target;
    
    void LateUpdate()
    {
        transform.position = Target.position; 
    }
}
