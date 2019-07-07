using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TrophyTile : MonoBehaviour
{
    [Serializable]
    public class OnWinEvent : UnityEvent { }

    [Serializable]
    public class OnPreWinEvent : UnityEvent { }

    [SerializeField]
    private OnWinEvent onWin = new OnWinEvent();
    public OnWinEvent OnWin => onWin;

    [SerializeField]
    private OnPreWinEvent onPreWin = new OnPreWinEvent();
    public OnPreWinEvent OnPreWin => onPreWin;

    public void OnTrigger(TileTrigger.TriggerInfo info)
    {
        // User wins game on trigger!
        if(info.enter)
        {
            onPreWin.Invoke();
            StartCoroutine(WinAnimation());
        }
    }

    private IEnumerator WinAnimation()
    {
        // Rotate, rise, then increase size a bit and disappear
        // I tried doing this with Unity's animation controller but jesus it's surprisingly hard to get
        // something to just rotate around its own axis. So whatever, this works and looks nice enough.
        // Should also add some speccy particles

        float start = Time.time;
        float end = start + 3.0f;
        float scaleOut = start + 2.0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + 3.0f, startPosition.z);

        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale * 3.0f;

        float t = 0.0f;
        float t2 = 0.0f;
        while(t < 1.0f)
        {
            t = Mathf.SmoothStep(0.0f, 1.0f, Mathf.InverseLerp(start, end, Time.time));
            t2 = Mathf.SmoothStep(0.0f, 1.0f, Mathf.InverseLerp(start, scaleOut, Time.time));

            // Always rotate
            transform.Rotate(0, Time.deltaTime * 720, 0);

            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            if(t2 < 1.0f)
            {
                transform.localScale = Vector3.Lerp(startScale, endScale, t2);
            }
            else
            {
                float t3 = Mathf.SmoothStep(0.0f, 1.0f, Mathf.InverseLerp(scaleOut, end, Time.time));
                transform.localScale = Vector3.Lerp(endScale, Vector3.zero, t3);
            }

            yield return null;
        }

        // All done, so tell the level manager we've hit a victory
        onWin.Invoke();
    }
}
