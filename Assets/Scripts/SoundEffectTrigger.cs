using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectTrigger : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    private void Start()
    {
        if(!source)
            source = GetComponent<AudioSource>();

        if (!source)
            return;

        source.loop = false;
        source.playOnAwake = false;
    }

    public void PlaySound(AudioClip inClip)
    {
        if (!source)
            return;

        source.Stop();
        source.clip = inClip;
        source.Play();
    }

    private void OnValidate()
    {
        if (source)
        {
            source.loop = false;
            source.playOnAwake = false;
        }
            
    }
}
