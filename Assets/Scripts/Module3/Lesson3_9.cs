using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lesson3_9 : MonoBehaviour
{
    int counter, task1Counter, income;
    [SerializeField] GameObject Lesson3__9, Task1, Task2,loanImg, drag, loanInfoBtn, Application,Approval,Suggestions;
    [SerializeField] Transform[] task1Items;
    [SerializeField] Button[] task2Buttons;
    [SerializeField] InputField[] inputFields;
    [SerializeField] Text task2HeadingText,repayment1,repayment2;


    // Start is called before the first frame update
    void Start()
    {
        task1Counter = 0;
        counter = 0;
        GameManagerModule3.instance.AddMoney(0);
        GameManagerModule3.instance.AddPoint(0);
        GameManagerModule3.instance.AddBankMoney(0, 0);
        GameManagerModule3.instance.LivesCheck();
    }
    public void Task3_9NextBtn() // next button in dialog box
    {
        // Debug.Log(counter);
        GameManagerModule3.instance.CurrentLevelProgress(counter, 8.0f);
        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 9)
        {
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Now, I know the different kinds of loans are used for different things, but what I need from you Alex is to help me pick what kind of loan I should take out for different needs. Can you help?";
            counter++;
        }
        else if (counter == 1)
        {

            Lesson3__9.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            counter++;
        }
        else if (counter == 2)
        {
            Lesson3__9.SetActive(false);
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "You have to sort loans from most to least expensive based on the total repayment amount and what they are needed for.";

            counter++;
        }
        else if (counter == 3)
        {
            Lesson3__9.SetActive(true);
            loanImg.SetActive(false);
            drag.SetActive(true);
            loanInfoBtn.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            if (task1Items[3].localPosition==new Vector3(-225, 136.3f,0) &&
                task1Items[2].localPosition == new Vector3(-75, 136.3f, 0) &&
                task1Items[1].localPosition == new Vector3(75, 136.3f, 0) &&
                task1Items[0].localPosition == new Vector3(225, 136.3f, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                GameManagerModule3.instance.Toast.Show("+10 Points", 2);
                GameManagerModule3.instance.AddPoint(10);
                counter++;
                Task3_9NextBtn();
            }
            else if(task1Counter>0)
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
            task1Counter++;


        }
        else if (counter == 4)
        {
            Task1.SetActive(false);
            Invoke("DialogueShowWithWait", 2);
            GameManagerModule3.instance.DialogueText.text = "I know that I have already asked you for too much, Alex. But if you could just please guide me through one of my loans I can do the rest myself…. please.";
            counter++;
        }
        else if (counter == 5)
        {
            Task1.SetActive(false);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task2.SetActive(true);
           // counter++;
        }
        else if (counter == 6)
        {
            Application.SetActive(false);
            Approval.SetActive(true);
            
            int.TryParse(inputFields[3].text, out income);
            Approval.transform.GetChild(1).GetComponent<Text>().text = "You can get loan of " + income*5;
            counter++;
        }
        else if (counter == 7)
        {
            Approval.SetActive(false);
            Suggestions.SetActive(true);
            int loan=income*5;
            int perMonth = loan / 12;
            int interest=(perMonth*5)/100;
            int total = interest + perMonth;
            repayment1.text = "You have to pay " + total + " every month for 12 months";
            perMonth = loan / 24;
            interest = (perMonth * 9) / 100;
            total = interest + perMonth;
            repayment2.text = "You have to pay " + total + " every month for 24 months";
        }

        else if (counter == 8)
        {
            Lesson3__9.SetActive(false);
            PlayerPrefs.SetInt("sofia", 10);
            PlayerPrefs.SetInt("gameprogress", 18);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.AddBankMoney(150, 1);
            GameManagerModule3.instance.AddPoint(15);
            
        }
    }

    public void ShowLoanBtn()
    {
        if (!loanImg.activeInHierarchy)
            loanImg.SetActive(true);
        else
            loanImg.SetActive(false);
    }
    public void DialogueShowWithWait()
    {
        GameManagerModule3.instance.NpcImages[2].SetActive(true);
        GameManagerModule3.instance.DialogueBox.SetActive(true);
    }

    public void task2Btns(int btnNo)
    {
        task2Buttons[0].transform.parent.gameObject.SetActive(false);
        Application.SetActive(true);
        if(btnNo == 0)
        {
            task2HeadingText.text = "Mortgage Loan";
        }
        else if (btnNo == 1)
        {
            task2HeadingText.text = "Personal Loan";
        }
        else if (btnNo == 2)
        {
            task2HeadingText.text = "Student Loan";
        }
    }
    public void VirtualAppSubmitBtn()
    {
        for (int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i].text == "")
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
                return;
            }
        }
        GameManagerModule3.instance.PopUpShow(0);
        counter++;
        Task3_9NextBtn();
    }

    public void RepaymentBtns(int btnNo)
    {
        if (btnNo == 0)
        {
            GameManagerModule3.instance.PopUpShow(1);
            if (PlayerPrefs.GetInt("lives") <= -1)
                GameManagerModule3.instance.LifeEnd();
        }
        else
        {
            GameManagerModule3.instance.PopUpShow(0);
            counter++;
            Task3_9NextBtn();
        }
    }

    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 9);
        PlayerPrefs.SetInt("gameprogress", 17);
        SceneManager.LoadScene(4);
    }
}
