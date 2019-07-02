using UnityEngine;

[CreateAssetMenu(menuName = "Actions/ProgramActionNull", fileName = "ProgramActionNull", order = 1)]
class ProgramActionNull : ProgramAction
{
    public override bool Invoke(Robot self, float time)
    {
        return false;
    }
}
