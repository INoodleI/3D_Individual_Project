using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingBehavior : RhinoBugBehavior
{
    private Transform cam;
    [SerializeField] private Transform playerSit;
    private bool run;
    public override Type Update()
    {
        Debug.Log("Riding Update: ");
        Movement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerController.instance.Dismount();
            return (typeof(WanderBehavior));
        }
        return GetType();
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //Debug.Log("Fast: "+brain.runSpeed);
            brain.agent.velocity = transform.forward * brain.runSpeed;
            brain.agent.angularSpeed = 30;
            if (run == false)
                brain.PlayAnim("Run");
            run = true;
        }
        else
        {
            brain.agent.angularSpeed = 720;
            if (run)
                brain.PlayAnim("Walk");
            run = false;
        }
        brain.agent.SetDestination(transform.position + cam.forward*2f * transform.parent.localScale.x);
    }

    public override void OnTrigger()
    {
        run = false;
    }

    public RidingBehavior(RhinoBugBrain b, Transform camera) : base(b)
    {
        cam = camera;
    }
}
