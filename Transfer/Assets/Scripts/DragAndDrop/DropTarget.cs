using System;
using UnityEngine;
using UnityEngine.Events;

public class DropTarget : MonoBehaviour
{
    [Serializable]
    public class OnDropEvent : UnityEvent<DropTarget, DragSource> { }

    [SerializeField]
    private AudioClip onDropSound;

    [SerializeField]
    private OnDropEvent onDrop;
    public OnDropEvent OnDrop => onDrop;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnDragSourceDropped(DragSource source)
    {
        if(enabled && source.gameObject != gameObject)
        {
            if(onDropSound)
            {
                audioSource.PlayOneShot(onDropSound, 2f);
            }

            onDrop.Invoke(this, source);
        }
    }
}
