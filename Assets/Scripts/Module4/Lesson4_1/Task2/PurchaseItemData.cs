// PurchaseItemData.cs - (Ensure itemName is set correctly)
using UnityEngine;

public class PurchaseItemData : MonoBehaviour
{
    [Tooltip("Unique Name of the item (MUST MATCH validation logic in Lesson4_2)")]
    public string itemName = "New Item"; // e.g., Groceries, Rent, Headphones, New Sneakers

    [Tooltip("Cost of this item")]
    public float cost = 0f;

    [Tooltip("The feedback tip shown when this item is categorized.")]
    [TextArea]
    public string feedbackTip = "Saving tip goes here!";

    [HideInInspector] public bool isSorted = false; // Track if placed

    // Basic position reset info
    private Vector3 startPosition;
    private Transform startParent;
    private bool positionStored = false;

    void Start()
    {
        StoreStartPosition();
    }

    void StoreStartPosition() {
         if (!positionStored) {
            startPosition = transform.position;
            startParent = transform.parent;
            positionStored = true;
        }
    }

    public void ResetPosition()
    {
        if (positionStored) {
            transform.SetParent(startParent, true);
            transform.position = startPosition;
            // isSorted = false; // Resetting isSorted might be needed if you want to retry after incorrect drop
        }
    }
    public Vector3 GetStartPosition() { StoreStartPosition(); return startPosition; }
    public Transform GetStartParent() { StoreStartPosition(); return startParent; }
}