using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Lesson3_3 : MonoBehaviour
{
    int counter;
    [SerializeField] GameObject Lesson3__3, Task1, Task2;
    string[] compareWords = { "Flight Booking", "Online Shopping", "Buying Groceries", "House Rent"};
    string[] cardsText = { "Builds Credit History", "Fraud Protection", "Unlimited Money", "Emergency Payments",
                           "No Repayment Needed","No fees"};
    [SerializeField] Text compareWordsText;
    [SerializeField] Text[] cardsTextObj;
    [SerializeField] Transform bgTask2;
    [SerializeField] Transform[] cards;
    [SerializeField] GameObject[] items;
    int compareCounter;

    // Start is called before the first frame update
    void Start()
    {
        compareCounter = 0;
        //defVal1 = 0;
        //task2counter = 0;
        counter = 0;
        GameManagerModule3.instance.AddMoney(0);
        GameManagerModule3.instance.AddPoint(0);
        GameManagerModule3.instance.AddBankMoney(0, 0);
        GameManagerModule3.instance.LivesCheck();
        GameManagerModule3.instance.NPCsNames.text = "Sofia";
        GameManagerModule3.instance.NpcImages[2].SetActive(true);
        for(int i = 0; i < 6; i++)
        {
            cardsTextObj[i].transform.parent.GetComponent<DragAndDrop>().enabled = false;
        }

    }
    public void Task3_3NextBtn() // next button in dialog box
    {
        GameManagerModule3.instance.CurrentLevelProgress(counter, 6.0f);
        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 3)
        {
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "I hear people get more than a single credit card, but that hasn't ever made sense to me, can you explain how I'll be able to manage my credit and debt if I do?";
            counter++;
        }
        else if (counter == 1)
        {
            Lesson3__3.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            counter++;
        }

        else if (counter == 2)
        {
            
            if (cards[0].gameObject.activeInHierarchy && cards[0].localPosition==new Vector3(-260, -222, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                cards[0].gameObject.SetActive(false);
                return;
            }      
            else if (cards[1].gameObject.activeInHierarchy && cards[1].localPosition == new Vector3(-260, -222, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                cards[1].gameObject.SetActive(false);
                return;
            }
            else if (cards[2].gameObject.activeInHierarchy && cards[2].localPosition == new Vector3(260, -222, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                cards[2].gameObject.SetActive(false);
                return;
            }
            else if (cards[3].gameObject.activeInHierarchy && cards[3].localPosition == new Vector3(-260, -222, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                cards[3].gameObject.SetActive(false);
                return;
            }
            else if (cards[4].gameObject.activeInHierarchy && cards[4].localPosition == new Vector3(260, -222, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                cards[4].gameObject.SetActive(false);
                return;
            }
            else if (cards[5].gameObject.activeInHierarchy && cards[5].localPosition == new Vector3(260, -222, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                cards[5].gameObject.SetActive(false);
                return;
            }

            for (int i = 0; i < 6; i++)
            {
                if (cards[i].gameObject.activeInHierarchy)
                {
                    GameManagerModule3.instance.PopUpShow(1);
                    if (PlayerPrefs.GetInt("lives") <= -1)
                        GameManagerModule3.instance.LifeEnd();
                    return;
                }
            }
            Task1.SetActive(false);
            //GameManagerModule3.instance.NpcImages[2].SetActive(true);
            //GameManagerModule3.instance.DialogueBox.SetActive(true);
            Invoke("DialogueShowWithWait", 1.5f);
            GameManagerModule3.instance.DialogueText.text = "Well done! You've mastered credit card basics.";
            GameManagerModule3.instance.Toast.Show("+10 Points", 1.5f);
            GameManagerModule3.instance.AddPoint(10);
            counter++;
        }
        else if (counter == 3)
        {

            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Okay, so now I have only have one more question, how can I make my budget for the week, and decide how to use debit and credit for which tasks?";
            counter++;
        }
        else if (counter == 4)
        {

            //Lesson3__3.SetActive(true);
            Task2.SetActive(true);
            //Task1.SetActive(false);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            counter++;
        }
        else if (counter == 5)
        {
            bgTask2.GetChild(1).gameObject.SetActive(false);
            bgTask2.GetChild(4).gameObject.SetActive(false);
            bgTask2.GetChild(2).gameObject.SetActive(true);
            bgTask2.GetChild(3).gameObject.SetActive(true);
            bgTask2.GetChild(5).gameObject.SetActive(true);

           
        }

        else if (counter == 6)
        {
            Lesson3__3.SetActive(false);
            //GameManagerModule3.instance.DialogueBox.SetActive(false);
            //GameManagerModule3.instance.NpcImages[3].SetActive(false);
            PlayerPrefs.SetInt("sofia", 4);
            PlayerPrefs.SetInt("gameprogress", 12);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.AddBankMoney(150, 1);
            GameManagerModule3.instance.AddPoint(10);
        }
    }

    public void CompareBtn(int bankOrcreditUnion)
    {
        if (compareCounter == 0 || compareCounter == 1 || compareCounter == 2)
        {
            if (bankOrcreditUnion == 0)
            {
                compareCounter++;
                GameManagerModule3.instance.PopUpShow(0);
                if (compareCounter < 4)
                {
                    compareWordsText.text = compareWords[compareCounter];
                    items[compareCounter].SetActive(true);
                    items[compareCounter-1].SetActive(false);
                    
                }
            }
            else if (bankOrcreditUnion == 1)
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
        }
        else if(compareCounter == 3)
        {
            if (bankOrcreditUnion == 1)
            {
                counter++;
                Task3_3NextBtn();
            }
            else if (bankOrcreditUnion == 0)
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
        }
    }
    public void CardsTextReveal(int cardNo)
    {
        cardsTextObj[cardNo].transform.parent.GetComponent<DragAndDrop>().enabled = true;
        if (cardNo == 0)
        {
            cardsTextObj[cardNo].text = cardsText[cardNo];
        }
        else if (cardNo == 1)
        {
            cardsTextObj[cardNo].text = cardsText[cardNo];
        }
        else if (cardNo == 2)
        {
            cardsTextObj[cardNo].text = cardsText[cardNo];
        }
        else if (cardNo == 3)
        {
            cardsTextObj[cardNo].text = cardsText[cardNo];
        }
        else if (cardNo == 4)
        {
            cardsTextObj[cardNo].text = cardsText[cardNo];
        }
        else if (cardNo == 5)
        {
            cardsTextObj[cardNo].text = cardsText[cardNo];
        }
    }
    public void DialogueShowWithWait()
    {
        GameManagerModule3.instance.NpcImages[2].SetActive(true);
        GameManagerModule3.instance.DialogueBox.SetActive(true);
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 3);
        PlayerPrefs.SetInt("gameprogress", 11);
        SceneManager.LoadScene(4);
    }
}
