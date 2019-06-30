using UnityEngine;

[CreateAssetMenu(menuName = "Programs/MoveProgram", fileName = "MoveProgram", order = 1)]
class MoveProgram : Program
{
    public override void Invoke(Robot self)
    {
        self.Move(1);
    }
}
