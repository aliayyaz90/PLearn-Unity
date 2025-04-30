using UnityEngine;
using UnityEngine.EventSystems;

// Enum to define the reasons for saving

public class GoalDropSlot : MonoBehaviour, IDropHandler
{
    [Tooltip("The reason this drop zone represents.")]
    public ReasonType reasonType;

    private Lesson4_Controller lessonController; // Reference to the main lesson script

    void Start()
    {
        // Find the main lesson controller in the scene
        lessonController = FindObjectOfType<Lesson4_Controller>();
        if (lessonController == null)
        {
            Debug.LogError("GoalDropSlot could not find the Lesson4_1 controller in the scene!");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"OnDrop detected on: {gameObject.name}");

        // Get the GameObject that was dragged
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject == null) return;

        // Check if the dropped object has the GoalData component
        GoalData goalData = droppedObject.GetComponent<GoalData>();
        if (goalData != null && lessonController != null)
        {
            Debug.Log($"Dropped Goal: {goalData.goalType}, Target Reason: {reasonType}");

            // Ask the lesson controller if this is the correct match
            bool isCorrect = lessonController.Task1_IsMatchCorrect(goalData.goalType, this.reasonType);

            if (isCorrect)
            {
                Debug.Log("Match is correct!");
                // Tell the lesson controller about the successful match
                lessonController.Task1_ProcessCorrectMatch(goalData, droppedObject, this.transform); // Pass transform for snapping
            }
            else
            {
                 Debug.Log("Match is incorrect!");
                // Optionally provide feedback for incorrect drop
                 lessonController.Task1_ProcessIncorrectMatch(goalData); // Tell controller it was wrong

                 // Snap the object back to its original position (handled by controller now)
                 // goalData.ResetPosition(); // Let controller handle reset
            }
        }
        else
        {
             Debug.LogWarning($"Dropped object {droppedObject.name} does not have GoalData component or Lesson4_1 controller not found.");
             // If it's a draggable goal without correct data, maybe reset it?
             droppedObject.GetComponent<GoalData>()?.ResetPosition();
        }
    }
}