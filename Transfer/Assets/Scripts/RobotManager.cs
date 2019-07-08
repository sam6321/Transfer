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


    public float NextUpdate { get; private set; } = 0;
    private List<Robot> robots = new List<Robot>();
    private Coroutine robotExecutingCoroutine = null;
    private bool force = false;

    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Robot robot = transform.GetChild(i).GetComponent<Robot>();
            if(robot)
            {
                robots.Add(robot);
            }
        }

        updateDelay = LevelManager.RobotSpeed;
        NextUpdate = Time.time + updateDelay;
    }

    private void Update()
    {
        bool shouldUpdate = (LevelManager.AutoAdvance && Time.time >= NextUpdate) || force;
        if (shouldUpdate && robotExecutingCoroutine == null)
        {
            robotExecutingCoroutine = StartCoroutine(ExecuteRobots());
            force = false;
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
        force = true;
    }
}
