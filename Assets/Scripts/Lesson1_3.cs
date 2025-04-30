using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Lesson1_3 : MonoBehaviour
{
    public int counterr;
    public Button[] PopUpOrder, PopUpDelivery;
    public GameObject CostOrder, DeliveryFee, Lesson1_3UI;
    bool select1, select2;
    void Start()
    {
        select1 = false;
        select2 = false;
        GameManager.instance.wallet.SetActive(true);
        GameManager.instance.LivesCheck();
        counterr = 0;
        GameManager.instance.AddMoney(0);
        GameManager.instance.AddPoint(0);
        GameManager.instance.Toast.Show("Go back to Grandma Millie with vegetables", 3);
       // Debug.Log(GameManager.instance.player.localPosition);
        GameManager.instance.NpcImages[0].SetActive(true);
    }
    public void Task3NextBtn() // next button in dialog box
    {
      //  Debug.Log(counterr);
        GameManager.instance.CurrentLevelProgress(counterr, 9.0f);
        if (counterr == 0 && PlayerPrefs.GetInt("grandma") == 3)
        {
            //PlayerPrefs.SetInt("grandma", 4);
            GameManager.instance.DialogPanel.SetActive(true);
            GameManager.instance.DialogueBox.SetActive(true);
            GameManager.instance.DialogueText.text = "Thankyou Alex for bringing the vegetables. Here is your reward 20.";
            GameManager.instance.AddMoney(20);
            GameManager.instance.Toast.Show("Money: +$20",3);
            counterr++;         
        }
        else if (counterr == 1)
        {
            GameManager.instance.DialogueText.text = "Now go to the baker Emma and bring me some cakes.";
            counterr++;
        }
        else if(counterr == 2)
        {
            GameManager.instance.DialogPanel.SetActive(false);
            counterr++;
        }
        else if (counterr == 3 && PlayerPrefs.GetInt("emma") == 2)
        {
            GameManager.instance.NpcImages[0].SetActive(false);
            GameManager.instance.NpcImages[2].SetActive(true);
            GameManager.instance.DialogPanel.SetActive(true);
            GameManager.instance.DialogueText.text = "Hello Alex. I just don't know where all the money keeps going, I mean, we're making so much money; business is booming, but why aren't we making a profit? You're old lady Millie's grandson, aren't you? Help me out, will ya?";
            PlayerPrefs.SetInt("emma", 1);
            counterr++;
        }
        else if(counterr == 4)
        {
            GameManager.instance.DialogPanel.transform.GetChild(1).gameObject.SetActive(true);
            GameManager.instance.DialogueBox.SetActive(false);
            Lesson1_3UI.SetActive(true);
            counterr++;
        }
        else if (counterr == 5)
        {
            GameManager.instance.DialogPanel.transform.GetChild(1).GetComponent<Image>().enabled = false;
            GameManager.instance.nextButton.gameObject.SetActive(false);
            GameManager.instance.DialogueBox.SetActive(true);
            Lesson1_3UI.transform.GetChild(0).gameObject.SetActive(false);
            GameManager.instance.DialogueText.text = "Please help me choosing the correct option in the popup cards to reduce my expenses.";
            counterr++;
        }
        else if(counterr == 6)
        {
            Lesson1_3UI.SetActive(true);
            GameManager.instance.DialogueText.text = "My orders €1,500 worth of supplies but only uses €1,100. That’s a €300 loss in excess inventory.";
            PopUpOrder[0].transform.parent.gameObject.SetActive(true);          
            if (!select1)
                GameManager.instance.Toast.Show("Select an option!",2);           
            else
                counterr++;
        }
        else if (counterr == 7)
        {
            GameManager.instance.DialogueText.text = "I am paying €600 for deliveries. That’s too high!";
            PopUpOrder[0].transform.parent.gameObject.SetActive(false);
            PopUpDelivery[0].transform.parent.gameObject.SetActive(true);
            
            if (!select2)
                GameManager.instance.Toast.Show("Select an option!", 2);
            else
                counterr++;
        }
        else if(counterr == 8)
        {
            PopUpDelivery[0].transform.parent.gameObject.SetActive(false);
            GameManager.instance.DialogueText.text = "Thankyou Alex for reducing my expences. Here take some freshly baked cakes.";
            counterr++;
        }
        else if(counterr == 9)
        {
            GameManager.instance.DialogPanel.SetActive(false);
            PlayerPrefs.SetInt("gameprogress", 3);
            PlayerPrefs.SetInt("grandma", 4);
            GameManager.instance.RewardPanell();
            GameManager.instance.AddMoney(500);
            GameManager.instance.AddPoint(20);
        } 
    }
    public void OrderCostBtn()
    {
        PopUpOrder[0].GetComponent<Image>().color = Color.green;
        PopUpOrder[0].interactable = false;
        PopUpOrder[1].interactable = false;
        PopUpOrder[2].interactable = false;
        PopUpOrder[3].interactable = false;
        counterr++;
        select1 = true;
        GameManager.instance.PopUpShow(0);
    }
    public void DeliveryFeeBtn()
    {
        PopUpDelivery[2].GetComponent<Image>().color = Color.green;
        PopUpDelivery[2].interactable = false;
        PopUpDelivery[0].interactable = false;
        PopUpDelivery[1].interactable = false;
        PopUpDelivery[3].interactable = false;
        counterr++;
        select2 = true;
        GameManager.instance.PopUpShow(0);
    }
    public void OrderrWrong(int WrongOrderBtnNO)
    {
        select1 = true;
        PopUpOrder[WrongOrderBtnNO].GetComponent<Image>().color = Color.red;
        GameManager.instance.PopUpShow(1);
        if (PlayerPrefs.GetInt("lives") <= -1)
              GameManager.instance.LifeEnd();
            GameManager.instance.DialogueText.text = "You selected the wrong option. Reduce over ordering supplies and try again.";

        if (PlayerPrefs.GetInt("walletpoint") >= 5)
        {
            GameManager.instance.Toast.Show("Points: -5", 3);
            GameManager.instance.RemovePoint(5);
        }

    }
    public void DeliveryWrong(int WrongDeliveryBtnNO)
    {
        select2 = true;
        PopUpDelivery[WrongDeliveryBtnNO].GetComponent<Image>().color = Color.red;
        GameManager.instance.PopUpShow(1);
        GameManager.instance.DialogueText.text = "You selected the wrong option. Reduce delivery rate and try again.";
        if (PlayerPrefs.GetInt("lives") <= -1)
            GameManager.instance.LifeEnd();
        if (PlayerPrefs.GetInt("walletpoint") >= 5)
        {
            GameManager.instance.Toast.Show("Points: -5", 3);
            GameManager.instance.RemovePoint(5);
        }
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("emma", 0);
        PlayerPrefs.SetInt("grandma", 1);
        PlayerPrefs.SetInt("gameprogress", 2);
        SceneManager.LoadScene(2);
    }
}