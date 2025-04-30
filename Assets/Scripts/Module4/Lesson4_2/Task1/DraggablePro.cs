using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePro : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
    public TextMeshProUGUI proText;
    public SavingsAccountType assignedAccount;
    private Vector2 originalPos;
    private CanvasGroup canvasGroup;

    public void Initialize(string text, SavingsAccountType account) 
    {
        proText.text = text;
        assignedAccount = account;
        originalPos = transform.position;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) 
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) 
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) 
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if (!eventData.pointerEnter?.GetComponent<DropZone>()) 
        {
            ResetPosition();
        }
    }

    public void ResetPosition() 
    {
        transform.position = originalPos;
    }
}