using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBudgetItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BudgetCategory category;
    private Lesson4_3_Controller controller;
    private Vector2 originalPosition;

    public void Initialize(Lesson4_3_Controller controller)
    {
        this.controller = controller;
        originalPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        
        if(!eventData.pointerEnter.CompareTag("BudgetDropZone"))
        {
            transform.position = originalPosition;
        }
        else
        {
            controller.HandleBudgetItemDrop(category);
        }
    }
}