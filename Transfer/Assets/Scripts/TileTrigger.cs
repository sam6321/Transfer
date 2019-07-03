using System;
using UnityEngine;
using UnityEngine.Events;

public class TileTrigger : MonoBehaviour
{
    [Serializable]
    public class OnTriggerEvent : UnityEvent<TileTrigger, Robot> { }

    public enum TriggerResponseType
    {
        MoveOver, // The robot should move over this trigger
        StopOn // The robot should stop on this trigger instead of moving over it
    }

    [SerializeField]
    private TriggerResponseType responseType;
    public TriggerResponseType ResponseType => responseType;

    [SerializeField]
    private OnTriggerEvent onTrigger = new OnTriggerEvent();
    public OnTriggerEvent OnTrigger => onTrigger;

    public void InvokeTrigger(Robot robot)
    {
        onTrigger.Invoke(this, robot);
    }
}
