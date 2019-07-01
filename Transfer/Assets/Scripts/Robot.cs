﻿using System.Collections;
using UnityEngine;

public abstract class Robot : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The time each Move or Rotate should take, in seconds.")]
    private float transformTime = 0.1f;

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
    
    public void InvokeCurrentProgram(float stepDelay)
    {
        if(programCoroutine != null)
        {
            programCoroutine = StartCoroutine(RunProgram(program, stepDelay));
        }
    }

    private IEnumerator RunProgram(Program program, float stepDelay)
    {
        for (int i = 0; i < program.Actions.Count; i++)
        {
            program.Actions[i].Invoke(this);
            if (i != program.Actions.Count - 1)
            {
                yield return new WaitForSeconds(stepDelay);
            }
        }
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
    public void Move(int steps)
    {
        Vector3 target = transform.position + transform.forward * steps;
        target.y = 0;
        // Check if there's a tile here to walk on
        if(tileManager.TileExists(target))
        {
            StartCoroutine(MoveCoroutine(steps));
        }
    }

    /// <summary>
    /// Rotate the robot left or right this many steps.
    /// Each step is 90 degrees.
    /// Positive numbers move right, negative numbers move left.
    /// </summary>
    /// <param name="steps">Rotation steps</param>
    public void Rotate(int steps)
    {
        StartCoroutine(RotateCoroutine(steps));
    }

    /// <summary>
    /// Use the robot's special ability.
    /// </summary>
    /// <returns></returns>
    public void UseAbility()
    {
        StartCoroutine(UseAbilityCoroutine());
    }

    protected abstract IEnumerator UseAbilityCoroutine();

    private IEnumerator MoveCoroutine(int steps)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position + transform.forward * steps;
        float startTime = Time.time;
        float targetTime = startTime + transformTime;

        while (transform.position != targetPosition)
        {
            float t = Mathf.InverseLerp(startTime, targetTime, Time.time);
            t = Mathf.SmoothStep(0.0f, 1.0f, t);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
    }

    private IEnumerator RotateCoroutine(int steps)
    {
        float startRotation = transform.eulerAngles.y;
        float targetRotation = transform.eulerAngles.y + steps * 90;
        float startTime = Time.time;
        float targetTime = startTime + transformTime;

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
