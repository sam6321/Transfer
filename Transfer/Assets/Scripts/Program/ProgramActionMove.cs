using UnityEngine;

[CreateAssetMenu(menuName = "Actions/ProgramActionMove", fileName = "ProgramActionMove", order = 1)]
class ProgramActionMove : ProgramAction
{
    public int steps = 1;

    public override void Invoke(Robot self)
    {
        self.Move(steps);
    }
}
