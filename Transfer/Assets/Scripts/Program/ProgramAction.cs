using UnityEngine;

public abstract class ProgramAction : ScriptableObject
{
    public Sprite icon;
    public string displayName;

    /// <summary>
    /// Invoke this program action
    /// </summary>
    /// <param name="self">The robot to invoke the action on</param>
    /// <param name="time">The time that this action should take</param>
    /// <returns>True if the action should be waited for, false if not</returns>
    public abstract bool Invoke(Robot self, float time);
}
