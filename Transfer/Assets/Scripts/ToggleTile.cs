using System;
using UnityEngine;
using UnityEngine.Events;

public class ToggleTile : MonoBehaviour
{
    [Serializable]
    public class OnToggleEvent : UnityEvent<bool> { }

    [Serializable]
    public class OnToggleOnEvent : UnityEvent { }

    [Serializable]
    public class OnToggleOffEvent : UnityEvent { }

    [SerializeField]
    private AudioClip onToggleOnSound;

    [SerializeField]
    private AudioClip onToggleOffSound;

    [SerializeField]
    [Tooltip("The toggled state")]
    private bool toggled = false;
    public bool Toggled
    {
        get => toggled;
        set
        {
            if(value != toggled)
            {
                toggled = value;

                if(toggled)
                {
                    onToggleOn.Invoke();
                    if(onToggleOnSound)
                    {
                        audioSource.PlayOneShot(onToggleOnSound);
                    }
                }
                else
                {
                    OnToggleOff.Invoke();
                    if (onToggleOffSound)
                    {
                        audioSource.PlayOneShot(onToggleOffSound);
                    }
                }

                onToggle.Invoke(toggled);
                Animate();
            }
        }
    }

    [SerializeField]
    [Tooltip("If set, toggle state will be changed on a robot entering the trigger")]
    private bool toggleOnEnter = false;
    public bool ToggleOnEnter { get => toggleOnEnter; set => toggleOnEnter = value; }

    [SerializeField]
    [Tooltip("If set, toggle state will be changed on a robot leaving the trigger")]
    private bool toggleOnLeave = false;
    public bool ToggleOnLeave { get => toggleOnLeave; set => toggleOnLeave = value; }

    [SerializeField]
    [Tooltip("True if the trigger is a lever, which is only activated along the local x-axis")]
    private bool isLever = false;

    [SerializeField]
    [Tooltip("Fired when the toggle changes")]
    private OnToggleEvent onToggle = new OnToggleEvent();
    public OnToggleEvent OnToggle => onToggle;

    [SerializeField]
    [Tooltip("Fired when the toggle is toggled on")]
    private OnToggleOnEvent onToggleOn = new OnToggleOnEvent();
    public OnToggleOnEvent OnToggleOn => onToggleOn;

    [SerializeField]
    [Tooltip("Fired when the toggle is toggled off")]
    private OnToggleOffEvent onToggleOff = new OnToggleOffEvent();
    public OnToggleOffEvent OnToggleOff => onToggleOff;

    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Animate();
    }

    public void Toggle()
    {
        Toggled = !Toggled;
    }

    private void Animate()
    {
        if (animator)
        {
            animator.SetBool("toggled", toggled);
        }
    }

    private void OnTrigger(TileTrigger.TriggerInfo info)
    {
        if((info.enter && toggleOnEnter) || (!info.enter && toggleOnLeave))
        {
            if(isLever)
            {
                LeverTrigger(info);
            }
            else
            {
                Toggle();
            }
        }
    }

    private void LeverTrigger(TileTrigger.TriggerInfo info)
    {
        Vector3 dir = transform.InverseTransformDirection(transform.position - info.handle.transform.position);
        Debug.Log(dir);
        if(dir.x > 0.1f)
        {
            Toggled = false;
        }
        else if(dir.x < -0.1f)
        {
            Toggled = true;
        }
    }
}
