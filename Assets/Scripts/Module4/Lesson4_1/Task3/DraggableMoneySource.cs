using UnityEngine;
using UnityEngine.EventSystems;

// Enum to define the origin of the money being dragged

[RequireComponent(typeof(CanvasGroup))] // Ensures CanvasGroup exists
public class DraggableMoneySource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Tooltip("The type of money source this represents.")]
    public MoneySourceType sourceType;

    [Tooltip("The amount this object represents (should match repair cost).")]
    public float amount = 500f;

    // Internal components and state
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas; // Root canvas for dragging
    private Vector3 startPosition;
    private Transform startParent;
    private bool positionStored = false;

    void Awake() // Use Awake for component initialization
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        StoreStartPosition(); // Store initial position early

        // Find the root canvas for proper drag rendering
        Transform testCanvas = transform.parent;
        while (testCanvas != null) {
            canvas = testCanvas.GetComponent<Canvas>();
            if (canvas != null) break;
            testCanvas = testCanvas.parent;
        }
        if(canvas == null) Debug.LogError("DraggableMoneySource requires a Canvas ancestor!");
    }

     // Stores the starting position and parent transform
     void StoreStartPosition() {
         if (!positionStored) {
            startPosition = transform.position; // Using world position
            startParent = transform.parent;
            positionStored = true;
        }
    }

    // Called when dragging starts
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Prevent dragging if this script component is disabled (e.g., after successful drop)
        if(!this.enabled) {
            eventData.pointerDrag = null; // Cancel drag
            return;
        }

        StoreStartPosition(); // Make sure position is known

        canvasGroup.alpha = 0.6f; // Make semi-transparent during drag
        canvasGroup.blocksRaycasts = false; // Allow raycasts to hit drop zones beneath
        transform.SetParent(canvas.transform, true); // Move to top level of canvas for visibility
        transform.SetAsLastSibling(); // Ensure it renders above other UI elements
    }

    // Called continuously while dragging
    public void OnDrag(PointerEventData eventData)
    {
         if(!this.enabled) return; // Don't move if disabled

        // Update position based on input and canvas type
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay) {
            rectTransform.position = eventData.position;
        } else {
            // Adjust for Camera or World Space canvas
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var globalMousePos);
            rectTransform.position = globalMousePos;
        }
    }

    // Called when dragging ends
    public void OnEndDrag(PointerEventData eventData)
    {
        // If the script was disabled during the drop (e.g., successful payment), don't reset visuals/position
         if(!this.enabled) return;

        // Restore visuals
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        // Check if the drop target was the PaymentDropZone. If not, reset position.
        // PaymentDropZone's OnDrop handles the successful drop logic.
        if (eventData.pointerEnter == null || eventData.pointerEnter.GetComponent<PaymentDropZone>() == null)
        {
            ResetPosition();
        }
    }

    // Public method to reset the object to its starting position and parent
    public void ResetPosition() {
        if (positionStored) {
            transform.SetParent(startParent, true); // Reparent without changing world position initially
            transform.position = startPosition;     // Snap back to original world position
            // Debug.Log($"{gameObject.name} position reset to: {startPosition}");
        } else {
             Debug.LogWarning($"{gameObject.name} original position not stored, cannot reset properly.");
             // As a fallback, maybe try resetting to parent's center? Less reliable.
        }
    }
}