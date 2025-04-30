// CategoryDropZone.cs - (Updated OnDrop)
using UnityEngine;
using UnityEngine.EventSystems;

public class CategoryDropZone : MonoBehaviour, IDropHandler
{
    [Tooltip("The purchase category this zone represents.")]
    public PurchaseCategory category;

    private Lesson4_Controller lessonController;

    void Start()
    {
        lessonController = FindObjectOfType<Lesson4_Controller>();
        if (lessonController == null) {
            Debug.LogError($"CategoryDropZone ({category}) couldn't find Lesson4_2!");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject == null || lessonController == null) return;

        PurchaseItemData itemData = droppedObject.GetComponent<PurchaseItemData>();

        // Only process if the item exists and isn't already sorted
        if (itemData != null && !itemData.isSorted)
        {
            // *** ADD VALIDATION STEP ***
            bool isValid = lessonController.Task2_IsValidDrop(itemData, this.category);

            if (isValid) {
                // If valid, process the successful sort
                lessonController.Task2_ProcessItemSorted(itemData, droppedObject, this.category, this.transform);
            } else {
                // If invalid, tell the controller to handle the rejection
                lessonController.Task2_ProcessIncorrectDrop(itemData);
            }
        }
        // No action needed if item already sorted or has no data.
    }
}