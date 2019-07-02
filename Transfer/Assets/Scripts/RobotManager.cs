using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Robot update delay, in seconds between updates")]
    private float updateDelay = 0.5f;

    public float UpdateDelay
    {
        get => updateDelay;
        set => updateDelay = value;
    }

    [SerializeField]
    [Tooltip("Delay between steps in multi-step programs")]
    private float stepDelay = 0.1f;

    public float StepDelay
    {
        get => stepDelay;
        set => stepDelay = value;
    }

    [SerializeField]
    [Tooltip("Delay between robots taking action")]
    private float robotStepDelay = 0.5f;

    public float RobotStepDelay
    {
        get => robotStepDelay;
        set => robotStepDelay = value;
    }

    [SerializeField]
    [Tooltip("Time that each robot action should take")]
    private float robotActionTime = 0.1f;

    public float RobotActionTime
    {
        get => robotActionTime;
        set => robotActionTime = value;
    }

    [SerializeField]
    private List<Robot> robots = new List<Robot>();

    public float NextUpdate { get; private set; } = 0;

    private Coroutine robotExecutingCoroutine = null;

    private void Update()
    {
        if(Time.time >= NextUpdate && robotExecutingCoroutine == null)
        {
            robotExecutingCoroutine = StartCoroutine(ExecuteRobots());
        }
    }

    private IEnumerator ExecuteRobots()
    {
        foreach (Robot robot in robots)
        {
            // Spin in place here until the robot has finished its turn.
            bool completed = false;
            robot.InvokeCurrentProgram(robotActionTime, stepDelay, r => completed = true);
            yield return new WaitUntil(() => completed == true);
        }

        NextUpdate = Time.time + updateDelay;
        robotExecutingCoroutine = null;
    }

    public void ForceStep()
    {
        NextUpdate = Time.time;
    }
}
