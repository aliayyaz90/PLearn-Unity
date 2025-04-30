using UnityEngine;
using UnityEngine.UI; // If your feedback panel uses UI elements like Text

// Enum to define the types of life goals


public class GoalData : MonoBehaviour
{
    [Tooltip("The type of this life goal.")]
    public GoalType goalType;

    [Tooltip("The UI Panel containing the explanation and estimate for this goal when correctly matched.")]
    public GameObject feedbackPanel; // Assign the specific feedback panel for this goal in the Inspector

    // Optional: You could add references to specific Text fields within the panel
    // public Text explanationText;
    // public Text estimateText;

    private Vector3 startPosition;
    private Transform startParent;

    void Start()
    {
        // Store initial position and parent in case we need to snap back on wrong drop
        startPosition = transform.position;
        startParent = transform.parent;

        // Ensure feedback panel is initially hidden
        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(false);
        }
    }

    // Public method to reset position if a wrong drop occurs
    public void ResetPosition()
    {
        transform.position = startPosition;
        transform.SetParent(startParent, true); // Maintain world position during reparenting
    }

     // Public method to get the starting position
    public Vector3 GetStartPosition()
    {
        return startPosition;
    }
     // Public method to get the starting parent
    public Transform GetStartParent()
    {
        return startParent;
    }
}