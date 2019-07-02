using UnityEngine;

[CreateAssetMenu(menuName = "Actions/ProgramActionRotate", fileName = "ProgramActionRotate", order = 1)]
class ProgramActionRotate : ProgramAction
{
    public int steps = 1;

    public override bool Invoke(Robot self, float time)
    {
        self.Rotate(steps, time);
        return true;
    }
}
