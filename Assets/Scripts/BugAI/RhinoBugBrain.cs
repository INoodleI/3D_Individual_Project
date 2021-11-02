using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/*
 * State Machine Code structure from https://youtu.be/YdERlPfwUb0
 */
public class RhinoBugBrain : MonoBehaviour
{
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Animator animator;
    [SerializeField] public float animationScaleFactor;

    [SerializeField] public Transform home;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    [SerializeField] public Vector2 walkFromHomeRange;
    private Dictionary<Type, RhinoBugBehavior> behaviors;
    private RhinoBugBehavior currentState;

    [SerializeField] private bool updateAnimSpeed;
    
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Initializing Bug: "+name);
        InitializeBehaviorStates();
        Debug.Log("Bug state: " + currentState.GetType());
        animator.speed = animationScaleFactor;
        updateAnimSpeed = false;
    }

    private void InitializeBehaviorStates()
    {
        behaviors = new Dictionary<Type, RhinoBugBehavior>
        {
            {typeof(WanderBehavior), new WanderBehavior(this)},
            {typeof(ChaseBehavior), new ChaseBehavior(this)}
        };
        currentState = behaviors.Values.First();
        currentState.OnTrigger();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update: "+ currentState.GetType());
        Type nextState = currentState?.Update();
        Debug.Log(" - NextState = "+ nextState);
        
        if(nextState != null && nextState != currentState.GetType())
        {
            SwitchState(GetType());
        }

        if (updateAnimSpeed)
        {
            updateAnimSpeed = false;
            animator.speed = animationScaleFactor;
        }
    }

    private void SwitchState(Type nextState)
    {
        currentState = behaviors[nextState];
        currentState.OnTrigger();
    }

    public void PlayAnim(string anim)
    {
        switch (anim)
        {
            case "Idle":
                animator.CrossFadeInFixedTime("Idle", 0.1f);
                break;
            case "Walk":
                animator.CrossFadeInFixedTime("Walk Forward In Place", 0.1f);
                break;
            case "Run":
                animator.CrossFadeInFixedTime("Run Forward In Place", 0.1f);
                break;
            case "Die":
                animator.CrossFadeInFixedTime("Die", 0.1f);
                break;
            case "Stab":
                animator.CrossFadeInFixedTime("Stab Attack", 0.1f);
                break;
            case "Smash":
                animator.CrossFadeInFixedTime("Smash Attack", 0.1f);
                break;
            default:
                return;
        }
    }
}
