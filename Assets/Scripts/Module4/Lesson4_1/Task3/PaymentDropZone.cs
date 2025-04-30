using UnityEngine;
using UnityEngine.EventSystems; // Required for IDropHandler

public class PaymentDropZone : MonoBehaviour, IDropHandler // Implement the drop interface
{
    private Lesson4_Controller lessonController; // Reference to the main lesson script

    void Start()
    {
        // Find the active Lesson4_3 script instance in the scene
        lessonController = FindObjectOfType<Lesson4_Controller>();
        if (lessonController == null) {
            Debug.LogError("PaymentDropZone couldn't find the active Lesson4_3 script!");
        }
    }

    // This method is called automatically when a draggable object is dropped onto this GameObject
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag; // The object that was dragged
        if (droppedObject == null || lessonController == null) return; // Exit if nothing dropped or no controller

        // Try to get the DraggableMoneySource component from the dropped object
        DraggableMoneySource moneySource = droppedObject.GetComponent<DraggableMoneySource>();

        if (moneySource != null) {
            // If it's a valid money source, tell the lesson controller to process it
            lessonController.Task3_ProcessPaymentDrop(moneySource.sourceType, moneySource.amount, droppedObject);
        } else {
            // If the dropped object wasn't a money source, tell it to reset (if possible)
             Debug.LogWarning("Dropped object is not a valid Money Source.");
             droppedObject.SendMessage("ResetPosition", SendMessageOptions.DontRequireReceiver); // Attempt to reset its position
        }
    }
}