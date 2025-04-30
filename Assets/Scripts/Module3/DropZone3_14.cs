using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone3_14 : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var draggable = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggable != null)
        {
            draggable.MatchTo(gameObject);
        }
    }
}
