using System.Collections;
using System.Linq;
using UnityEngine;
using System;

public abstract class Robot : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The current program that the robot is running")]
    private Program program = null;

    public Program Program
    {
        get => program;
        set
        {
            if(program != value)
            {
                program = value;
                dragSource.Popup.GetComponent<ProgramPopup>().Program = program;
            }
        }
    }

    private Coroutine programCoroutine = null;

    private DragSource dragSource;
    private TileManager tileManager;

    private void Start()
    {
        tileManager = GameObject.Find("Tiles").GetComponent<TileManager>();
        dragSource = GetComponent<DragSource>();
        if(program != null)
        {
            dragSource.Popup.GetComponent<ProgramPopup>().Program = program;
        }
    }
    
    /// <summary>
    /// Invokes the current program of the robot
    /// </summary>
    /// <param name="stepTime">How long each step should take, in seconds</param>
    /// <param name="stepDelay">Delay between steps, in seconds</param>
    /// <param name="onProgramCompleted">Callback to call when the program has completed</param>
    public void InvokeCurrentProgram(float stepTime, float stepDelay, Action<Robot> onProgramCompleted=null)
    {
        if(programCoroutine == null)
        {
            programCoroutine = StartCoroutine(RunProgram(program, stepTime, stepDelay, onProgramCompleted));
        }
        else
        {
            Debug.LogError("InvokeCurrentProgram called too soon, program still running!");
        }
    }

    private IEnumerator RunProgram(Program program, float stepTime, float stepDelay, Action<Robot> onProgramCompleted)
    {
        for (int i = 0; i < program.Actions.Count; i++)
        {
            // Invoke this step for the provided duration, then wait only if the step will actually take time to complete.
            if(program.Actions[i].Invoke(this, stepTime))
            {
                // Wait for the step to finish
                yield return new WaitForSeconds(stepTime);
            }
            if (i != program.Actions.Count - 1)
            {
                // Wait for the delay between steps, but not for the last step 
                yield return new WaitForSeconds(stepDelay);
            }
        }

        programCoroutine = null;

        onProgramCompleted(this);
    }

    public void OnDropSwapPrograms(DropTarget target, DragSource source)
    {
        // Target = this, source = other.
        Robot otherRobot = source.GetComponent<Robot>();
        if(otherRobot)
        {
            Program otherProgram = otherRobot.Program;
            otherRobot.Program = Program;
            Program = otherProgram;
        }
    }

    /// <summary>
    /// Move the robot forward / backwards this many steps.
    /// Positive numbers move forward, negative numbers move backward.
    /// </summary>
    /// <param name="steps">Movement steps</param>
    /// <param name="time">Time to apply the movement over</param>
    /// <returns>True if the move will take place, false if the robot is blocked and will not move</returns>
    public bool Move(int steps, float time)
    {
        Vector3 target = transform.position + transform.forward * steps;
        target.y = 0;
        // Check if there's a tile here to walk on
        if(tileManager.TileExists(target))
        {
            StartCoroutine(MoveCoroutine(steps, time));
            return true;
        }
        return false;
    }

    /// <summary>
    /// Rotate the robot left or right this many steps.
    /// Each step is 90 degrees.
    /// Positive numbers move right, negative numbers move left.
    /// </summary>
    /// <param name="steps">Rotation steps</param>
    /// <param name="time">Time to apply the movement over</param>
    /// <returns>True if the rotation will take place, or false if the robot is blocked and will not rotate</returns>
    public bool Rotate(int steps, float time)
    {
        StartCoroutine(RotateCoroutine(steps, time));
        return true;
    }

    /// <summary>
    /// Use the robot's special ability.
    /// </summary>
    /// <returns>True if the ability use will take place, or false if the robot cannot use its ability</returns>
    public bool UseAbility()
    {
        StartCoroutine(UseAbilityCoroutine());
        return true;
    }

    protected abstract IEnumerator UseAbilityCoroutine();

    private IEnumerator MoveCoroutine(int steps, float time)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position + transform.forward * steps;
        float startTime = Time.time;
        float targetTime = startTime + time;

        while (transform.position != targetPosition)
        {
            float t = Mathf.InverseLerp(startTime, targetTime, Time.time);
            t = Mathf.SmoothStep(0.0f, 1.0f, t);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
    }

    private IEnumerator RotateCoroutine(int steps, float time)
    {
        float startRotation = transform.eulerAngles.y;
        float targetRotation = transform.eulerAngles.y + steps * 90;
        float startTime = Time.time;
        float targetTime = startTime + time;

        while (transform.eulerAngles.y != targetRotation)
        {
            float t = Mathf.InverseLerp(startTime, targetTime, Time.time);
            t = Mathf.SmoothStep(0.0f, 1.0f, t);
            float rotation = Mathf.LerpAngle(startRotation, targetRotation, t);
            transform.eulerAngles = new Vector3(0, rotation, 0);
            yield return null;
        }
    }
}
