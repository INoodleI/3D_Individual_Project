using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugRidingTrigger : MonoBehaviour
{
    [SerializeField] private PlayerController pc;

    private void OnTriggerEnter(Collider other)
    {
//        Debug.Log(other.name);
        if (other.tag == "Bug")
        {
            pc.EnableRiding(other.GetComponent<RhinoBugBrain>());
        }
    }
}
