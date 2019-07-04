using System;
using UnityEngine;
using UnityEngine.Events;

public class LeverTile : MonoBehaviour
{
    [Serializable]
    public class OnFlippedEvent : UnityEvent<bool> { }

    [Serializable]
    public class OnFlippedOnEvent : UnityEvent { }

    [Serializable]
    public class OnFlippedOffEvent : UnityEvent { }

    [SerializeField]
    private bool flipped = false;

    [SerializeField]
    private OnFlippedEvent onFlipped = new OnFlippedEvent();

    [SerializeField]
    private OnFlippedOnEvent onFlippedOn = new OnFlippedOnEvent();

    [SerializeField]
    private OnFlippedOffEvent onFlippedOff = new OnFlippedOffEvent();


    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("flipped", flipped);
    }

    public void OnTrigger(TileTrigger.TriggerInfo info)
    {
        if(info.enter)
        {
            flipped = !flipped;

            if(flipped)
            {
                onFlippedOn.Invoke();
            }
            else
            {
                onFlippedOff.Invoke();
            }

            onFlipped.Invoke(flipped);

            animator.SetBool("flipped", flipped);
        }
    }
}
