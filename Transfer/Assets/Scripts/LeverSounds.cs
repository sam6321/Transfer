using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip onDownSound;

    [SerializeField]
    private AudioClip onUpSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOnDownSound()
    {
        audioSource.PlayOneShot(onDownSound);
    }

    public void PlayOnUpSound()
    {
        audioSource.PlayOneShot(onUpSound);
    }
}
