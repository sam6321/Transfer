using System.Collections;
using UnityEngine;

public class DragSource : MonoBehaviour
{
    static GameObject currentDragPopup = null;

    [SerializeField]
    private RectTransform canvas;

    [SerializeField]
    private GameObject popupPrefab = null;

    private GameObject popup = null;
    private ProgramPopup programPopup = null;

    [SerializeField]
    private bool popupFollow = true;

    [SerializeField]
    private Vector2 popupOffset = Vector2.zero;

    private bool mousedOver = false;
    private bool drag = false;
    private bool returningHome = false;

    public GameObject Popup
    {
        get
        {
            if (!popup && popupPrefab)
            {
                popup = Instantiate(popupPrefab, canvas);
                programPopup = popup.GetComponent<ProgramPopup>();
            }

            return popup;
        }
    }

    private static bool popupsEnabled = true;

    public static void DisablePopups()
    {
        popupsEnabled = false;
    }

    public static void EnablePopups()
    {
        popupsEnabled = true;
    }

    private void LateUpdate()
    {
        if(!popupsEnabled)
        {
            drag = false;
            mousedOver = false;
            HideDragPopup();
            Destroy(popup);
        }
        else if(popup && programPopup.Shown)
        {
            if (!returningHome)
            {
                UpdatePopupPosition();
            }

            if (drag)
            {                
                if (drag && popupFollow)
                {
                    popup.transform.position = (Vector2)Input.mousePosition + popupOffset;
                }
                
                if(Input.GetMouseButtonUp(0))
                {
                    EndDrag();
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        if(popupsEnabled)
        {
            mousedOver = true;
            ShowDragPopup();
        }
    }

    private void OnMouseDrag()
    {
        if(popupsEnabled)
        {
            BeginDrag();
        }
    }

    private void OnMouseExit()
    {
        if(popupsEnabled)
        {
            mousedOver = false;
            HideDragPopup();
        }
    }

    private void ShowDragPopup()
    {
        if(!popupPrefab)
        {
            return;
        }

        if (!popup)
        {
            popup = Instantiate(popupPrefab, canvas);
            programPopup = popup.GetComponent<ProgramPopup>();
        }

        programPopup.Shown = true;

        UpdatePopupPosition();
    }

    private IEnumerator ReturnPopupCoroutine(float time)
    {
        Vector2 startPosition = popup.transform.position;
        Vector2 targetPosition = (Vector2)Camera.main.WorldToScreenPoint(transform.position) + popupOffset;
        float startTime = Time.time;
        float targetTime = startTime + time;

        returningHome = true;
        while ((Vector2)popup.transform.position != targetPosition)
        {
            float t = Mathf.InverseLerp(startTime, targetTime, Time.time);
            t = Mathf.SmoothStep(0.0f, 1.0f, t);
            popup.transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
        returningHome = false;

        if (!mousedOver)
        {
            HideDragPopup();
        }
    }

    private void HideDragPopup()
    {
        if(!popupPrefab || drag)
        {
            return;
        }

        programPopup.Shown = false;
    }

    private void BeginDrag()
    {
        drag = true;
        currentDragPopup = popup;
    }

    private void EndDrag()
    {
        drag = false;
        // Check for a drop target
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            DropTarget target = hit.collider.GetComponent<DropTarget>();
            if(target)
            {
                target.OnDragSourceDropped(this);
            }
        }

        currentDragPopup = null;

        if (!mousedOver)
        {
            StartCoroutine(ReturnPopupCoroutine(0.25f));
        }
    }

    private void UpdatePopupPosition()
    {
        popup.transform.position = (Vector2)Camera.main.WorldToScreenPoint(transform.position) + popupOffset;
        if(currentDragPopup && currentDragPopup != popup)
        {
            // updating the position of a popup, while also dragging a popup, so we want to move this popup out of the way
            RectTransform dragPopupTransform = currentDragPopup.transform as RectTransform;
            RectTransform popupTransform = popup.transform as RectTransform;

            Vector3 mouse = Input.mousePosition;
            float dragPopupLeftEdge = dragPopupTransform.position.x - dragPopupTransform.sizeDelta.x * 0.5f;
            float popupRightEdge = popupTransform.position.x + popupTransform.sizeDelta.x * 0.5f;
            float push = popupRightEdge - dragPopupLeftEdge;
            if(push > 0)
            {
                popup.transform.position -= new Vector3(push, 0);
            }
        }
    }
}
