using UnityEngine;

public class SwitchTile : MonoBehaviour
{
    [SerializeField]
    private Transform handle;

    bool triggered = false;

    public void OnTrigger(TileTrigger trigger, Robot robot)
    {
        triggered = !triggered;
        handle.eulerAngles = new Vector3(0, 0, triggered ? 45 : -45);
    }
}
