using UnityEngine;

public class TileTrigger : MonoBehaviour
{ 
    public class TriggerInfo
    {
        public Robot robot;
        public bool enter;
    }

    public enum TriggerResponseType
    {
        MoveOver, // The robot should move over this trigger
        StopOn // The robot should stop on this trigger instead of moving over it
    }

    [SerializeField]
    private TriggerResponseType responseType;
    public TriggerResponseType ResponseType => responseType;

    private void OnTriggerEnter(Collider collider)
    {
        Robot robot = collider.GetComponent<Robot>();
        if(robot)
        {
            SendMessage("OnTrigger", new TriggerInfo() { robot = robot, enter = true });
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        Robot robot = collider.GetComponent<Robot>();
        if(robot)
        {
            SendMessage("OnTrigger", new TriggerInfo() { robot = robot, enter = false });
        }
    }
}
