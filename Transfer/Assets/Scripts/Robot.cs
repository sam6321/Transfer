using System.Collections;
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
                dragSource.PopupPrefab = program.programPopupPrefab;
            }
        }
    }

    public bool IsRunningAbilityCoroutine => abilityCoroutine != null;
    public bool IsRunningTransformCoroutime => transformCoroutine != null;

    private Coroutine abilityCoroutine = null;
    private Coroutine transformCoroutine = null;

    private DragSource dragSource;

    private void Start()
    {
        dragSource = GetComponent<DragSource>();
        if(program)
        {
            dragSource.PopupPrefab = program.programPopupPrefab;
        }
    }
    
    public void InvokeCurrentProgram()
    {
        if(program != null)
        {
            program.Invoke(this);
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
        if(IsRunningTransformCoroutime)
        {
            print("Move called while transformCoroutine is running");
        }
        else
        {
            Vector3 target = transform.position + transform.forward * steps;
            if(Mathf.Abs(target.x) > 10 || Mathf.Abs(target.y) > 10 || Mathf.Abs(target.z) > 10)
            {
                return;
            }
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
        if(IsRunningTransformCoroutime)
        {
            print("Rotate called while transformCoroutine is running");
        }
        else
        {
            StartCoroutine(RotateCoroutine(steps));
        }
    }

    /// <summary>
    /// Use the robot's special ability.
    /// </summary>
    /// <returns></returns>
    public void UseAbility()
    {
        if(IsRunningAbilityCoroutine)
        {
            print("UseAbility called while abilityCouroutine is running");
        }
        else
        {
            StartCoroutine(UseAbilityCoroutine());
        }
    }

    protected abstract IEnumerator UseAbilityCoroutine();

    protected void FinishAbilityCoroutine()
    {
        abilityCoroutine = null;
    }

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

        transformCoroutine = null;
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

        transformCoroutine = null;
    }
}
