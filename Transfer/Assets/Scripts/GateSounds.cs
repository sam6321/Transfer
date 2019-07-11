using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip onOpenSound;

    [SerializeField]
    private AudioClip onCloseSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOnOpenSound()
    {
        audioSource.PlayOneShot(onOpenSound);
    }

    public void PlayOnCloseSound()
    {
        audioSource.PlayOneShot(onCloseSound);
    }
}
