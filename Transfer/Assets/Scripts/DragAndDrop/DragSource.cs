using UnityEngine;

public class DragSource : MonoBehaviour
{
    [SerializeField]
    private RectTransform canvas;

    [SerializeField]
    private GameObject popupPrefab = null;

    private GameObject popup = null;

    [SerializeField]
    private bool popupFollow = true;

    [SerializeField]
    private Vector2 popupOffset = Vector2.zero;

    private bool mousedOver = false;
    private bool drag = false;

    public GameObject Popup
    {
        get
        {
            if (!popup && popupPrefab)
            {
                popup = Instantiate(popupPrefab, canvas);
                popup.SetActive(false);
            }

            return popup;
        }
    }

    private void LateUpdate()
    {
        if(popup && popup.activeSelf)
        {
            popup.transform.position = (Vector2)Camera.main.WorldToScreenPoint(transform.position) + popupOffset;
            if (drag)
            {
                if(popupFollow)
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
        mousedOver = true;
        ShowDragPopup();
    }

    private void OnMouseDrag()
    {
        BeginDrag();
    }

    private void OnMouseExit()
    {
        mousedOver = false;
        HideDragPopup();
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
        }

        if (!popup.activeSelf)
        {
            popup.SetActive(true);
        }
    }

    private void HideDragPopup()
    {
        if(!popupPrefab || drag)
        {
            return;
        }

        if(Popup.activeSelf)
        {
            Popup.SetActive(false);
        }
    }

    private void BeginDrag()
    {
        drag = true;
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

        if (!mousedOver)
        {
            HideDragPopup();
        }
    }
}
