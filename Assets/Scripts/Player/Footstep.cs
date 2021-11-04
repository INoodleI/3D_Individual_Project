using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    [SerializeField] private List<AudioSource> sound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
            sound[Random.Range(0,sound.Count)].Play();
    }
}
