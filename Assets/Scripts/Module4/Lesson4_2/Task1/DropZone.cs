using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler 
{
    public SavingsAccountType zoneType;
    public Lesson4_2_Controller controller;

    public void OnDrop(PointerEventData eventData) 
    {
        DraggablePro pro = eventData.pointerDrag?.GetComponent<DraggablePro>();
        if (pro != null) 
        {
            controller.HandleProDrop(pro, this);
        }
    }

    public void PlayCorrectFeedback() 
    {
        GetComponent<Image>().color = Color.green; // Example feedback
    }
}