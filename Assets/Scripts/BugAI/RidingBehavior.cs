using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingBehavior : RhinoBugBehavior
{
    private Transform cam;
    [SerializeField] private Transform playerSit;
    public override Type Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Fast: "+brain.runSpeed);
            brain.agent.velocity = brain.agent.velocity.normalized * brain.runSpeed;
            brain.agent.angularSpeed = 180;
            brain.PlayAnim("Walk");
        }
        else
        {
            brain.agent.angularSpeed = 720;
            brain.PlayAnim("Walk");
        }
        brain.agent.SetDestination(transform.position + cam.forward*4f);
        if (Input.GetKeyDown(KeyCode.Space))
            return (typeof(WanderBehavior));
        return GetType();
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
    }

    public RidingBehavior(RhinoBugBrain b, Transform camera) : base(b)
    {
        cam = camera;
    }
}
