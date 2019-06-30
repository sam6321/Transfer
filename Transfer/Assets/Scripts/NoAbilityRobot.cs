using System.Collections;

class NoAbilityRobot : Robot
{
    protected override IEnumerator UseAbilityCoroutine()
    {
        return null;
    }
}
