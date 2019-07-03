using System;
using UnityEngine;
using UnityEngine.Events;

public class TileUse : MonoBehaviour
{
    [Serializable]
    public class OnTileUseEvent : UnityEvent<TileUse, Robot> { }


    [SerializeField]
    private OnTileUseEvent onUse = new OnTileUseEvent();
    public OnTileUseEvent OnUse => onUse;

    public void InvokeUse(Robot robot)
    {
        onUse.Invoke(this, robot);
    }
}
