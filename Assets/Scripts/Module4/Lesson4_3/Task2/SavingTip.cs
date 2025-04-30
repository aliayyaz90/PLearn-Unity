using UnityEngine;

[System.Serializable]
public class SavingTip
{
    public string description;
    public bool isGood;  // true = good for saving
    public GameObject uiElement;  // Reference to UI object for drag-drop
}
