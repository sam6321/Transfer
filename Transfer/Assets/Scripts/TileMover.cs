using System.Collections;
using UnityEngine;

public class TileMover : MonoBehaviour
{
    public enum TileMoveType
    {
        Block, // When something else hits us, it should stop infront of us
        Push, // When something else tries to move into us, it should push us
        MoveThrough // When something else tries to move into it, it should overlap us and sit on top
    }

    [SerializeField]
    private AudioClip rotateStartSound;

    [SerializeField]
    private AudioClip rotateEndSound;

    [SerializeField]
    private AudioClip moveStartSound;

    [SerializeField]
    private AudioClip moveEndSound;

    [SerializeField]
    [Tooltip("When something tries to move into us, this is how we respond")]
    private TileMoveType moveType = TileMoveType.MoveThrough;
    public TileMoveType MoveType { get => moveType; set => moveType = value; }

    [SerializeField]
    [Tooltip("If set, this mover will push other movers. If not, this mover will be blocked by other movers")]
    private bool canPush = true;
    public bool CanPush { get => canPush; set => canPush = value; }

    private TileManager tileManager;
    private AudioSource audioSource;

    private void Start()
    {
        tileManager = GameObject.Find("Tiles").GetComponent<TileManager>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Move the mover this many steps in this direction over this time.
    /// Positive numbers move forward, negative numbers move backward.
    /// </summary>
    /// <param name="direction">The direction we should move from where we are now</param>
    /// <param name="steps">Movement steps</param>
    /// <param name="time">Time to apply the movement over</param>
    /// <returns>Number of steps we actually ended up moving</returns>
    public int Move(Vector3 direction, int steps, float time)
    {
        // Move in the direction, then play the moving animation once we've
        // decided where we can move.
        steps = tileManager.RunMove(transform.position, direction, steps, time, canPush);

        if (steps > 0)
        {
            StartCoroutine(MoveCoroutine(direction, steps, time));
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

    private IEnumerator MoveCoroutine(Vector3 direction, int steps, float time)
    {
        tileManager.UnlinkObject(gameObject);

        if(moveStartSound)
        {
            audioSource.PlayOneShot(moveStartSound);
        }

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position + direction * steps;
        float startTime = Time.time;
        float targetTime = startTime + time;

        while (transform.position != targetPosition)
        {
            float t = Mathf.InverseLerp(startTime, targetTime, Time.time);
            t = Mathf.SmoothStep(0.0f, 1.0f, t);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        if (moveEndSound)
        {
            audioSource.PlayOneShot(moveEndSound);
        }

        tileManager.LinkObject(gameObject);
    }

    private IEnumerator RotateCoroutine(int steps, float time)
    {
        if(rotateStartSound)
        {
            audioSource.PlayOneShot(rotateStartSound);
        }

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

        if(rotateEndSound)
        {
            audioSource.PlayOneShot(rotateEndSound);
        }
    }
}
