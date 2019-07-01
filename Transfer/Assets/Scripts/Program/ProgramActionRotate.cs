using UnityEngine;

[CreateAssetMenu(menuName = "Actions/ProgramActionRotate", fileName = "ProgramActionRotate", order = 1)]
class ProgramActionRotate : ProgramAction
{
    public int steps = 1;

    public override void Invoke(Robot self)
    {
        self.Rotate(steps);
    }
}
