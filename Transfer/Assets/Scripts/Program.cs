using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Program : ScriptableObject
{
    public Sprite icon;
    public string programName;
    public GameObject programPopupPrefab;
    public abstract void Invoke(Robot self);
}
