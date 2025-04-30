using UnityEngine;
using UnityEngine.EventSystems;

public class DropZoneTip : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool isGoodZone;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (dropped == null) return;

        SavingTipReference tipRef = dropped.GetComponent<SavingTipReference>();
        if (tipRef == null) return;

        tipRef.controller.EvaluateTip(tipRef, isGoodZone);
    }
}
