using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestScript : MonoBehaviour
{
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Transform target;

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up + transform.forward * 0.5f, Vector3.down, out hit, 4f, LayerMask.GetMask("Ground")))
        {
            transform.LookAt(transform.position+transform.forward);
        }
    }
}