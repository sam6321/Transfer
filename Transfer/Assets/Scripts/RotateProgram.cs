using UnityEngine;

[CreateAssetMenu(menuName = "Programs/RotateProgram", fileName = "RotateProgram", order = 1)]
class RotateProgram : Program
{
    public override void Invoke(Robot self)
    {
        self.Rotate(1);
    }
}
