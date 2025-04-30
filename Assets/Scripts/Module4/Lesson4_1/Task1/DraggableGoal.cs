using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // If using UI elements and Canvas

[RequireComponent(typeof(CanvasGroup))] // Useful for ignoring raycasts while dragging
public class DraggableGoal : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;
    private Transform startParent;
    private Canvas canvas; // Reference to the root Canvas

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        // Find the root canvas
        Transform testCanvas = transform.parent;
        while(testCanvas != null)
        {
            canvas = testCanvas.GetComponent<Canvas>();
            if (canvas != null) break;
            testCanvas = testCanvas.parent;
        }

        if (canvas == null)
        {
            Debug.LogError("DraggableGoal requires a Canvas ancestor!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Store starting position and parent
        startPosition = rectTransform.position;
        startParent = transform.parent;

        // Lift the object visually and make it ignore raycasts so drop zone can be detected
        canvasGroup.alpha = 0.6f; // Make it slightly transparent
        canvasGroup.blocksRaycasts = false; // Allow raycasts to pass through to objects underneath

        // Bring to front (optional but good practice for UI dragging)
        transform.SetParent(canvas.transform, true); // Reparent to root canvas to render on top
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the object with the mouse/finger
        // Adjust for canvas scaling mode if necessary
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            rectTransform.position = eventData.position;
        }
        else // For ScreenSpaceCamera or WorldSpace
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var globalMousePos);
            rectTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore visual state
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        // If not dropped on a valid slot, it might snap back.
        // The DropSlot or LessonController will handle correct drops.
        // If eventData.pointerDrag points to null here it means it wasn't dropped on anything that handled it.
        // We might need logic here OR in the drop slot to handle snapping back.
        // Let's handle snap back in the LessonController if the match is incorrect or if dropped nowhere specific.
         if (eventData.pointerEnter == null || eventData.pointerEnter.GetComponent<GoalDropSlot>() == null)
        {
             // Dropped outside a valid drop zone, reset position
            GetComponent<GoalData>()?.ResetPosition(); // Use GoalData's reset
        }
        // If dropped on a valid zone, GoalDropSlot will handle it
    }
}