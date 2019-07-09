using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(TileMover))]
public class Robot : MonoBehaviour
{
    [SerializeField]
    private string colourMaterialName = "Material.007";

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
    private TileMover mover;
    private TileManager tileManager;

    private void Start()
    {
        tileManager = GameObject.Find("Tiles").GetComponent<TileManager>();
        dragSource = GetComponent<DragSource>();
        mover = GetComponent<TileMover>();
        if (program != null)
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
    /// Use the robot's special ability.
    /// </summary>
    /// <returns>True if the ability use will take place, or false if the robot cannot use its ability</returns>
    public bool UseAbility()
    {
        //StartCoroutine(UseAbilityCoroutine());
        return false;
    }

    public int Move(int steps, float time)
    {
        return mover.Move(transform.forward, steps, time);
    }

    public bool Rotate(int steps, float time)
    {
        return mover.Rotate(steps, time);
    }
}
