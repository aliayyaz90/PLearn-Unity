using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone4 : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableBudgetItem item = eventData.pointerDrag.GetComponent<DraggableBudgetItem>();
        if(item != null)
        {
            item.transform.position = transform.position;
        }
    }
}