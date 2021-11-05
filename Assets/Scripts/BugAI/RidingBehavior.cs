using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingBehavior : RhinoBugBehavior
{
    private Transform cam;
    [SerializeField] private Transform playerSit;
    private bool run;
    private float attackingCooldown;
    public override Type Update()
    {
        Debug.Log("Riding Update: ");
        if (attackingCooldown > 0)
        {
            NoMove();
            attackingCooldown -= Time.deltaTime;
            if (attackingCooldown <= 0)
            {
                if(run)
                    brain.PlayAnim("Run");
                else
                    brain.PlayAnim("Walk");
                Movement();
            }
        }
        else
        {
            Movement();
            CheckForAttack();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerController.instance.Dismount();
            return (typeof(WanderBehavior));
        }
        return GetType();
    }

    private void NoMove()
    {
        brain.agent.velocity = Vector3.zero;
    }

    private void Attack()
    {
        attackingCooldown = 0.5f;
        brain.PlayAnim("Stab");
    }

    private void CheckForAttack()
    {
        if(Input.GetKeyDown(KeyCode.F))
            Attack();
    }

    public override void SpecialEvent(int eventNum)
    {
        Debug.Log("Poke");
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
        brain.agent.SetDestination(transform.position + cam.forward*3f);
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
