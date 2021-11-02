using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RhinoBugBehavior
{
    protected RhinoBugBrain brain;
    protected GameObject gameObject;
    protected Transform transform;

    public RhinoBugBehavior(RhinoBugBrain b)
    {
        brain = b;
        gameObject = brain.gameObject;
        transform = brain.transform;
    }

    public abstract Type Update();

    public virtual void OnTrigger()
    {
        return;
    }
}
