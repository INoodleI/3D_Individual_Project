using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingBehavior : RhinoBugBehavior
{
    private Transform cam;
    public override Type Update()
    {
        brain.agent.SetDestination(transform.position + cam.forward*2f);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return typeof(WanderBehavior);
        }
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
