using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour, IDropHandler
{
    private Vector2 NamePos = new Vector2(-315, 62);
    private Vector2 SSNPos = new Vector2(315, 62);
    private Vector2 DOBPos = new Vector2(-315, -133);
    private Vector2 AddressPos = new Vector2(315, -133);
    private Vector2 StudentProofPos = new Vector2(0, -37);
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("Item Dropped on Slot");

        // Get the dragged object
        GameObject draggedObject = eventData.pointerDrag;

        if (draggedObject != null)
        {
            // Snap the dragged object to this slot
            RectTransform draggedRectTransform = draggedObject.GetComponent<RectTransform>();
            draggedRectTransform.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                if (GameManager.instance.GetComponent<Lesson1_1>().enabled)//SceneManager.GetActiveScene().buildIndex == 2
                {
                    if (GameManager.instance.GetComponent<Lesson1_1>().ExpensesArr[GameManager.instance.GetComponent<Lesson1_1>().expenceItem].GetComponent<RectTransform>().anchoredPosition.x == -210
                    && GameManager.instance.GetComponent<Lesson1_1>().expenceItem == 3 ||
                    GameManager.instance.GetComponent<Lesson1_1>().ExpensesArr[GameManager.instance.GetComponent<Lesson1_1>().expenceItem].GetComponent<RectTransform>().anchoredPosition.x == 210
                    && GameManager.instance.GetComponent<Lesson1_1>().expenceItem != 3)
                    {
                        GameManager.instance.GetComponent<Lesson1_1>().ExpensesArr[GameManager.instance.GetComponent<Lesson1_1>().expenceItem].GetComponent<Image>().raycastTarget = false;
                        GameManager.instance.slider1.gameObject.SetActive(true);
                        GameManager.instance.slider1.value = 250;
                        Lesson1_1.wrongCatagory = false;
                    }
                    else
                    {
                        GameManager.instance.slider1.gameObject.SetActive(true);                            //change
                        Lesson1_1.wrongCatagory = true;
                    }
                }
                else if (GameManager.instance.GetComponent<Lesson1_2>().enabled) //SceneManager.GetActiveScene().buildIndex == 3
                {
                    if (GameManager.instance.GetComponent<Lesson1_2>().NeedsNWantsArr[GameManager.instance.GetComponent<Lesson1_2>().shoppingItem].GetComponent<RectTransform>().anchoredPosition.x == -210
                    && (GameManager.instance.GetComponent<Lesson1_2>().shoppingItem < 2 || GameManager.instance.GetComponent<Lesson1_2>().shoppingItem == 4) ||
                    GameManager.instance.GetComponent<Lesson1_2>().NeedsNWantsArr[GameManager.instance.GetComponent<Lesson1_2>().shoppingItem].GetComponent<RectTransform>().anchoredPosition.x == 210
                    && (GameManager.instance.GetComponent<Lesson1_2>().shoppingItem >= 2 && GameManager.instance.GetComponent<Lesson1_2>().shoppingItem < 4))
                    {
                        GameManager.instance.GetComponent<Lesson1_2>().NeedsNWantsArr[GameManager.instance.GetComponent<Lesson1_2>().shoppingItem].GetComponent<Image>().raycastTarget = false;
                        GameManager.instance.slider2.gameObject.SetActive(true);
                        GameManager.instance.slider2.value = 350;
                        Lesson1_2.wrongCatagory2 = false;
                    }
                    else
                    {
                        GameManager.instance.slider2.gameObject.SetActive(true);            //change
                        Lesson1_2.wrongCatagory2 = true;
                    }
                }
            }
            else if(SceneManager.GetActiveScene().buildIndex == 3)
            {
                if (GameManagerModule2.instance.GetComponent<Lesson2_4>().enabled) //SceneManager.GetActiveScene().buildIndex == 3
                {
                    if (GameManagerModule2.instance.GetComponent<Lesson2_4>().JordanInfo[0].GetComponent<RectTransform>().anchoredPosition == NamePos &&
                        GameManagerModule2.instance.GetComponent<Lesson2_4>().JordanInfo[1].GetComponent<RectTransform>().anchoredPosition == SSNPos &&
                        GameManagerModule2.instance.GetComponent<Lesson2_4>().JordanInfo[2].GetComponent<RectTransform>().anchoredPosition == DOBPos &&
                        GameManagerModule2.instance.GetComponent<Lesson2_4>().JordanInfo[3].GetComponent<RectTransform>().anchoredPosition == AddressPos &&
                        GameManagerModule2.instance.GetComponent<Lesson2_4>().JordanInfo[4].GetComponent<RectTransform>().anchoredPosition == StudentProofPos)
                        GameManagerModule2.instance.GetComponent<Lesson2_4>().WrongCategoryJordan = false;
                    else
                        GameManagerModule2.instance.GetComponent<Lesson2_4>().WrongCategoryJordan = true;
                }
            }

            

        }        
    }
}