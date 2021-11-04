using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Dictionary<Rigidbody, List<Vector3>> startingPositions;

    public void Initialize()
    {
        startingPositions = new Dictionary<Rigidbody, List<Vector3>>();
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            List<Vector3> temp = new List<Vector3>();
            temp.Add(rb.transform.localPosition);
            temp.Add(rb.transform.localRotation.eulerAngles);
            startingPositions.Add(rb, temp);
        }
    }

    public void Enable()
    {
        foreach (Rigidbody rb in startingPositions.Keys)
        {
            rb.isKinematic = false;
        }
        gameObject.SetActive(true);
    }
    
    public void Disable()
    {
        foreach (Rigidbody rb in startingPositions.Keys)
        {
            List<Vector3> temp = startingPositions[rb];
            rb.transform.localPosition = temp[0];
            rb.transform.localRotation = Quaternion.Euler(temp[1]);
            rb.isKinematic = true;
        }
        gameObject.SetActive(false);
    }
}
