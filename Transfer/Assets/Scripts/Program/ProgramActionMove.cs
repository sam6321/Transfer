using UnityEngine;

[CreateAssetMenu(menuName = "Actions/ProgramActionMove", fileName = "ProgramActionMove", order = 1)]
class ProgramActionMove : ProgramAction
{
    public int steps = 1;

    public override bool Invoke(Robot self, float time)
    {
        return self.Move(steps, time) > 0;
    }
}
