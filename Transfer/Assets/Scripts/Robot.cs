using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Robot : MonoBehaviour
{
    private class TriggerFireInfo
    {
        public TileTrigger trigger; // The trigger to fire
        public int pastStep; // The step that this trigger should fire on
    }

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
        // Step and collect triggers
        List<TriggerFireInfo> triggers = new List<TriggerFireInfo>();
        steps = CheckMove(steps, triggers);

        if (steps > 0)
        {
            StartCoroutine(MoveCoroutine(steps, time, triggers));
            return true;
        }
        return false;
    }

    private int CheckMove(int steps, IList<TriggerFireInfo> triggers)
    {
        // Check all tiles along the way and move to the closest one along the movement path.
        for (int i = 0; i < steps; i++)
        {
            Vector3 target = transform.position + transform.forward * (i + 1);
            Vector3 targetGround = new Vector3(target.x, 0, target.z);

            if (!tileManager.TileExists(targetGround))
            {
                return i; // stop here because there's nothing infront of us
            }

            // Check if the robot will move over a trigger of some sort. If it will, check the type of trigger
            TileTrigger trigger = tileManager.GetTileComponent<TileTrigger>(target);
            if (trigger)
            {
                // Add this trigger to be fired.
                triggers.Add(new TriggerFireInfo() { trigger = trigger, pastStep = i});

                // This will move over this trigger
                switch (trigger.ResponseType)
                {
                    default:
                    case TileTrigger.TriggerResponseType.MoveOver:
                        // Move over this tile
                        break;

                    case TileTrigger.TriggerResponseType.StopOn:
                        // The move should stop here on this trigger
                        return i + 1;
                }
            }
        }

        return steps;
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

    private IEnumerator MoveCoroutine(int steps, float time, IList<TriggerFireInfo> triggers)
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

            // Check if this position has moved on or past the triggers
            for(int i = triggers.Count - 1; i >= 0; i--)
            {
                TriggerFireInfo fire = triggers[i];
                if(t >= fire.pastStep / (float)steps)
                {
                    // We're on top of or past the trigger now so fire that bad boy!
                    fire.trigger.InvokeTrigger(this);
                    triggers.RemoveAt(i);
                }
            }

            yield return null;
        }
    }

    private IEnumerator RotateCoroutine(int steps, float time)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.AngleAxis(steps * 90, Vector3.up);
        float startTime = Time.time;
        float targetTime = startTime + time;

        while (transform.rotation != targetRotation)
        {
            float t = Mathf.InverseLerp(startTime, targetTime, Time.time);
            t = Mathf.SmoothStep(0.0f, 1.0f, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }
    }
}
