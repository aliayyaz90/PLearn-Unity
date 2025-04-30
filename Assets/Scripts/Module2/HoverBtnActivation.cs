using UnityEngine;
using UnityEngine.EventSystems;

public class HoverBtnActivation : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public GameObject[] hoverBtns;
    public GameObject hover;
    public void OnPointerEnter(PointerEventData eventData)
    {
        for(int i = 0; i < hoverBtns.Length; i++)
        {
            hoverBtns[i].SetActive(false);
        }
        hover.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }
}