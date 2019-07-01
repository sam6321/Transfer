using UnityEngine;

public abstract class ProgramAction : ScriptableObject
{
    public Sprite icon;
    public string displayName;
    public abstract void Invoke(Robot self);
}
