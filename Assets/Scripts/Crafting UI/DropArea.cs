using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler
{

    public virtual void OnDrop(PointerEventData eventData)
    {
        // check if there is already an item at the slot
        if (transform.childCount == 0)
        {
            // set new item position
            eventData.pointerDrag.transform.SetParent(this.transform);
            eventData.pointerDrag.transform.localPosition = Vector3.zero;

            Debug.Log(eventData.pointerDrag.name + " dropped at " + this.gameObject.name);
        }
    }
}