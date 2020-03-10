using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 lastMousePosition;
    [HideInInspector] public Vector2 originalPosition;
    public bool isInDropArea;

    void Start()
    {
        originalPosition = this.GetComponent<RectTransform>().position;
        isInDropArea = false;
    }

    // begin drag
    // save original position
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        lastMousePosition = eventData.position;
        isInDropArea = false;
        Cursor.visible = false;
    }

    // drag object
    public void OnDrag(PointerEventData eventData)
    {
        isInDropArea = false;
        Cursor.visible = false;

        Vector2 currentMousePosition = eventData.position;
        Vector2 diff = currentMousePosition - lastMousePosition;
        RectTransform rect = GetComponent<RectTransform>();

        Vector3 newPosition = rect.position + new Vector3(diff.x, diff.y, transform.position.z);
        Vector3 oldPos = rect.position;
        rect.position = newPosition;
        if (!IsRectTransformInsideSreen(rect))
        {
            rect.position = oldPos;
        }
        lastMousePosition = currentMousePosition;
    }

    // end of mouse drag
    // return to original position
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        Cursor.visible = true;
        //this.GetComponent<RectTransform>().position = originalPosition;
        if (isInDropArea == false)
        {
            this.GetComponent<RectTransform>().position = originalPosition;
        }
    }

    // make sure dragging object remains inside panel
    private bool IsRectTransformInsideSreen(RectTransform rectTransform)
    {
        bool isInside = false;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        int visibleCorners = 0;
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        foreach (Vector3 corner in corners)
        {
            if (rect.Contains(corner))
            {
                visibleCorners++;
            }
        }
        if (visibleCorners == 4)
        {
            isInside = true;
        }
        return isInside;
    }
}