using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lesson3_8 : MonoBehaviour
{
    int counter,task1Counter, task2Counter, loanAmount, repaymentTime,case1,case2;
    [SerializeField] GameObject Lesson3__8, Task1, Task2, Task3;
    [SerializeField] Transform[] task1Items;
    [SerializeField] GameObject[] task2Items;
    [SerializeField] Text loanAmountTextGet, repaymentTimeTextGet, loanAmountText1, loanAmountText2, repaymentTimeText1, repaymentTimeText2,
                          perMonth1,perMonth2;


    // Start is called before the first frame update
    void Start()
    {
        task1Counter = 0;
        task2Counter = 0;
        counter = 0;
        GameManagerModule3.instance.AddMoney(0);
        GameManagerModule3.instance.AddPoint(0);
        GameManagerModule3.instance.AddBankMoney(0, 0);
        GameManagerModule3.instance.LivesCheck();
    }
    public void Task3_8NextBtn() // next button in dialog box
    {
        // Debug.Log(counter);
        GameManagerModule3.instance.CurrentLevelProgress(counter, 7.0f);
        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 8)
        {
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Alex, I am considering taking out another loan to repair my kitchen after a pipe burst. But I need to understand if I can afford it and how will I be able to pay it off.";
            counter++;
        }
        else if (counter == 1)
        {

            Lesson3__8.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            if (task1Items[0].localPosition==new Vector3(-220, -312, 0)&&
                task1Items[1].localPosition == new Vector3(0, -312, 0) &&
                task1Items[2].localPosition == new Vector3(220, -312, 0) &&
                task1Items[3].localPosition == new Vector3(220, -152, 0) &&
                task1Items[4].localPosition == new Vector3(0, -152, 0) &&
                task1Items[5].localPosition == new Vector3(-220, -152, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                counter++;
                Task1.SetActive(false);
                Task3_8NextBtn();
            }
            else if(task1Counter>0)
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
            task1Counter++;
        }
        else if (counter == 2)
        {
            GameManagerModule3.instance.Toast.Show("+10 Points", 2);
            GameManagerModule3.instance.AddPoint(10);
            Invoke("DialogueShowWithWait", 2);
            GameManagerModule3.instance.DialogueText.text = "Here are 3 medium-sized expenses. Choose whether to use a loan or credit card for each.";
            counter++;
        }
        else if (counter == 3)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task2.SetActive(true);
            task2Items[task2Counter].SetActive(true);
            //counter++;
        }
        else if (counter == 4)
        {
            GameManagerModule3.instance.DialogueText.text = "Use the loan advisor bot to calculate the total cost of the loan.";
            counter++;
        }
        else if (counter == 5)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task3.SetActive(true);
            counter++;
        }
        else if (counter == 6)
        {
            loanAmount= int.Parse(loanAmountTextGet.text);
            repaymentTime= int.Parse(repaymentTimeTextGet.text);
            Task3.transform.GetChild(0).gameObject.SetActive(false);
            Task3.transform.GetChild(1).gameObject.SetActive(true);

            repaymentTimeText1.text = "Repayment Time: " + repaymentTime;
            repaymentTimeText2.text = "Repayment Time: " + repaymentTime*2;
            Task3Calculation();
            loanAmountText1.text = "Total Amount: " + case1;
            loanAmountText2.text = "Total Amount: " + case2;
            //counter++;
        }

        else if (counter == 7)
        {
            Lesson3__8.SetActive(false);
            PlayerPrefs.SetInt("sofia", 9);
            PlayerPrefs.SetInt("gameprogress", 17);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.AddBankMoney(100, 1);
            GameManagerModule3.instance.AddPoint(15);
            counter++;
        }
    }



    public void Task2Btns(int btnNo)
    {
        if(task2Counter == 0|| task2Counter == 1 || task2Counter == 4)
        {
            if (btnNo == 0)
            {
                GameManagerModule3.instance.PopUpShow(0);
                if (task2Counter < 4)
                {
                    task2Counter++;
                    task2Items[task2Counter].SetActive(true);
                    task2Items[task2Counter-1].SetActive(false);
                    
                }
                else
                {
                    Task2.SetActive(false);
                    GameManagerModule3.instance.Toast.Show("+10 Points",2);
                    GameManagerModule3.instance.AddPoint(10);
                    counter++;
                    Task3_8NextBtn();
                    Invoke("DialogueShowWithWait", 2);
                }
            }
            else
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
        }
        else if(task2Counter == 2 || task2Counter == 3)
        {
            if (btnNo == 1)
            {
               // if (task2Counter < 4)
               // {
                    task2Counter++;
                    task2Items[task2Counter].SetActive(true);
                    task2Items[task2Counter - 1].SetActive(false);
                    GameManagerModule3.instance.PopUpShow(0);
                //}
                //else
                //{
                //    counter++;
                //    Task3_8NextBtn();
                //}
            }
            else
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
        }

    }
    
    public void Task3Calculation()
    {
        int perMonth=loanAmount / repaymentTime;
        int interest = (Int32)(perMonth * 6.5f / 100);
        case1 = (perMonth + interest) * repaymentTime;
        Debug.Log(interest);
        int a = perMonth + interest;
        perMonth1.text = "Monthly Payment: " + a;

        perMonth = loanAmount / (repaymentTime*2);
        interest = (Int32)(perMonth * 9 / 100);
        case2 = (perMonth + interest) * (repaymentTime*2);
        a = perMonth + interest;
        perMonth2.text = "Monthly Payment: " + a;
    }
    
    public void task3Btns(int btnNo)
    {
        if(btnNo == 0)
        {
            GameManagerModule3.instance.PopUpShow(1);
            if (PlayerPrefs.GetInt("lives") <= -1)
                GameManagerModule3.instance.LifeEnd();
        }
        else
        {
            GameManagerModule3.instance.PopUpShow(0);
            counter++;
            Task3_8NextBtn();
        }
    }
    public void DialogueShowWithWait()
    {
        GameManagerModule3.instance.NpcImages[2].SetActive(true);
        GameManagerModule3.instance.DialogueBox.SetActive(true);
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 8);
        PlayerPrefs.SetInt("gameprogress", 16);
        SceneManager.LoadScene(4);
    }
}
