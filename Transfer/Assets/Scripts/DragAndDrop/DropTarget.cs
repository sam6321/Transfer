using System;
using UnityEngine;
using UnityEngine.Events;

public class DropTarget : MonoBehaviour
{
    [Serializable]
    public class OnDropEvent : UnityEvent<DropTarget, DragSource> { }

    [SerializeField]
    private OnDropEvent onDrop;
    public OnDropEvent OnDrop => onDrop;

    public void OnDragSourceDropped(DragSource source)
    {
        if(enabled && source.gameObject != gameObject)
        {
            onDrop.Invoke(this, source);
        }
    }
}
