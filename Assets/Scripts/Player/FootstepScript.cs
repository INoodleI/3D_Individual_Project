using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FootstepScript : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private List<AudioClip> sound;

    public void Footstep(float volume)
    {
        Debug.Log("Footstep");
        source.PlayOneShot(sound[Random.Range(0,sound.Count)],volume);
    }

    public void TripleFootstep()
    {
        source.PlayOneShot(sound[Random.Range(0,sound.Count)],0.6f);
        source.PlayOneShot(sound[Random.Range(0,sound.Count)],0.6f);
        source.PlayOneShot(sound[Random.Range(0,sound.Count)],0.6f);
    }
}
