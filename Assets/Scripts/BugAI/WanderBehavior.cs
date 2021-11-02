using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WanderBehavior : RhinoBugBehavior
{
    private Vector3 target;
    private bool waiting;
    private float waitingTime;
    public WanderBehavior(RhinoBugBrain b) : base(b)
    {
    }

    public override Type Update()
    {
        Debug.Log(" -- WanderState Update: waiting = "+waiting + ",  target = "+waitingTime + "  stop: "+brain.agent.isStopped);
        if (waiting)
        {
            waitingTime -= Time.deltaTime;
            if (waitingTime >= 0)
                return GetType();
        }

        brain.agent.speed = brain.walkSpeed;
        if (target == null || brain.agent.remainingDistance <= 0.5f)
        {
            CalculateNewTarget();
        }
        
        brain.agent.SetDestination(target);

        return GetType();
    }

    public override void OnTrigger()
    {
        brain.agent.speed = brain.walkSpeed;
        CalculateNewTarget();
    }

    private void CalculateNewTarget()
    {
        if (!waiting && Random.Range(0f, 1f) <= 0.4f)
        {
            waiting = true;
            waitingTime = Random.Range(2f, 5f);
            brain.agent.speed = 0;
            brain.PlayAnim("Idle");
        }
        else
        {
            waiting = false;
            brain.PlayAnim("Walk");
            Vector3 newPos;
            NavMeshHit hit;
            float maxDist = 3f;
            do
            {
                float x = Random.Range(-1f, 1f);
                float y = Random.Range(-1f, 1f);
                Vector3 randomDir = new Vector3(x, 0, y).normalized;
                newPos = brain.home.position + randomDir * Random.Range(brain.walkFromHomeRange.x, brain.walkFromHomeRange.y);
                Debug.Log(" -- RandomDir: "+randomDir + "   newPos: "+newPos);
                maxDist++;
            } while (!NavMesh.SamplePosition(newPos, out hit, maxDist, NavMesh.AllAreas));

            target = hit.position;
        }
    }
}
