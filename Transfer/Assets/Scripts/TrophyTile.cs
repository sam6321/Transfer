using UnityEngine;

public class TrophyTile : MonoBehaviour
{
    public void OnTrigger(TileTrigger trigger, Robot robot)
    {
        // User wins game on trigger!
        Debug.Log("You win fam");
    }
}
