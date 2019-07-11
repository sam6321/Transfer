using UnityEngine;

public class TileTrigger : MonoBehaviour
{ 
    public class TriggerInfo
    {
        public GameObject handle;
        public bool enter;
        public bool isRobot;
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
        SendMessage("OnTrigger", new TriggerInfo() { handle = collider.gameObject, enter = true, isRobot = collider.GetComponent<Robot>() != null });
    }

    private void OnTriggerExit(Collider collider)
    {
        SendMessage("OnTrigger", new TriggerInfo() { handle = collider.gameObject, enter = false, isRobot = collider.GetComponent<Robot>() != null });
    }
}
