using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Robot update delay, in seconds between updates")]
    private float updateDelay = 0.5f;

    [SerializeField]
    private List<Robot> robots = new List<Robot>();

    private void Start()
    {
        StartCoroutine(RobotUpdate());
    }

    private IEnumerator RobotUpdate()
    {
        while(true)
        {
            foreach(Robot robot in robots)
            {
                robot.InvokeCurrentProgram();
            }
            yield return new WaitForSeconds(updateDelay);
        }
    }
}
