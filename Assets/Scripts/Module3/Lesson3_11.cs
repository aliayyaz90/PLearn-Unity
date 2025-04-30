using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lesson3_11 : MonoBehaviour
{
    int counter, task1Counter,task2Counter;
    [SerializeField] GameObject Lesson3__11, Task1, Task2, Task3,ExplainPanel;
    [SerializeField] Transform[] task1Items;
    [SerializeField] GameObject[] task3Images;
    [SerializeField] Text[] ItemsText;
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
    public void Task3_11NextBtn() // next button in dialog box
    {
        GameManagerModule3.instance.CurrentLevelProgress(counter, 8.0f);
        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 11)
        {
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Alex, can you please deal with all this, technical stuff, please, please, please?";
            counter++;
        }
        else if (counter == 1)
        {

            Lesson3__11.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            counter++;
        }
        else if (counter == 2)
        {
            Task1.SetActive(false);
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Sort the debts according to avalanche method in which higher interest debt is paid off first.";
            counter++;
        }
        else if (counter == 3)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task1.SetActive(true);
            Task1.transform.GetChild(0).gameObject.SetActive(false);
            Task1.transform.GetChild(1).gameObject.SetActive(true);
            if (task1Items[0].localPosition == new Vector3(-225, 136.3f, 0) &&
                task1Items[1].localPosition == new Vector3(-75, 136.3f, 0) &&
                task1Items[3].localPosition == new Vector3(75, 136.3f, 0) &&
                task1Items[2].localPosition == new Vector3(225, 136.3f, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                GameManagerModule3.instance.Toast.Show("+15 Points", 2);
                GameManagerModule3.instance.AddPoint(15);
                counter++;
                Task3_11NextBtn();
            }
            else if (task1Counter > 0)
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
            GameManagerModule3.instance.DialogueText.text = "Manish so hard to keep track of please, Alex, can you make me a calendar that I can follow?";
            counter++;
        }
        else if (counter == 5)
        {
            Task2.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false); 
           // counter++;
        }
        else if (counter == 6)
        {
            Task2.SetActive(false);
            GameManagerModule3.instance.Toast.Show("+15 Points", 2);
            Invoke("DialogueShowWithWait", 2);
            GameManagerModule3.instance.DialogueText.text = "Alex, I'm suffering a medical emergency, you have to reallocate funds to make sure that everything stays normal.";
            counter++;
        }
        else if (counter == 7)
        {
            Task3.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            //counter++;
        }

        else if (counter == 8)
        {
            Lesson3__11.SetActive(false);
            PlayerPrefs.SetInt("sofia", 12);
            PlayerPrefs.SetInt("gameprogress", 20);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.AddBankMoney(100, 1);
            GameManagerModule3.instance.AddPoint(25);
            counter++;
        }
    }

    public void ShowDebtBtn(int btnNo)
    {
        if (btnNo==0)
            ExplainPanel.SetActive(true);
        
        else if(btnNo==1)
            ExplainPanel.SetActive(false);
    }

    public void Task2Btns(int BtnNo)
    {
        
        if (task2Counter == 0)
        {
            if(BtnNo == 0)
            {
                ItemsText[0].text = "Rent (750)";
                ItemsText[1].text = "Groceries (300)";
                ItemsText[2].text = "Student Loan (150)";
                ItemsText[3].text = "Utilities (150)";
                ItemsText[4].text = "Mobile (100)";
                ItemsText[5].text = "Dining Out (200)";
                ItemsText[6].text = "Entertainment (200)";
                ItemsText[7].text = "Transport (100)";
                GameManagerModule3.instance.PopUpShow(0);
                task2Counter = 1;
            }
            else
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
        }
        else if(task2Counter == 1)
        {
            if (BtnNo == 1)
            {
                ItemsText[0].text = "Rent (800)";
                ItemsText[1].text = "Groceries (300)";
                ItemsText[2].text = "Student Loan (200)";
                ItemsText[3].text = "Utilities (200)";
                ItemsText[4].text = "Mobile (150)";
                ItemsText[5].text = "Dining Out (250)";
                ItemsText[6].text = "Entertainment (200)";
                ItemsText[7].text = "Transport (150)";
                GameManagerModule3.instance.PopUpShow(0);
                task2Counter = 2;
            }
            else
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
        }
        else if (task2Counter == 2)
        {
            if (BtnNo == 2)
            {
                GameManagerModule3.instance.PopUpShow(0);
                counter++;
                Task3_11NextBtn();
            }
            else
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
        }
    }
    public void DialogueShowWithWait()
    {
        GameManagerModule3.instance.NpcImages[2].SetActive(true);
        GameManagerModule3.instance.DialogueBox.SetActive(true);
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 11);
        PlayerPrefs.SetInt("gameprogress", 19);
        SceneManager.LoadScene(4);
    }

    public void Task3Btns(int btnNo)
    {
        if (task3Images[0].activeInHierarchy)
        {
            if (btnNo == 0)
            {
                GameManagerModule3.instance.PopUpShow(0);
                task3Images[0].SetActive(false);
                task3Images[1].SetActive(true);
            }
            else
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
        }
        else if(task3Images[1].activeInHierarchy || task3Images[2].activeInHierarchy)
        {
            if(btnNo == 1)
            {
                GameManagerModule3.instance.PopUpShow(0);
                if (task3Images[1].activeInHierarchy)
                {
                    task3Images[1].SetActive(false);
                    task3Images[2].SetActive(true);
                }
                else if (task3Images[2].activeInHierarchy)
                {
                    counter++;
                    Task3_11NextBtn();
                }
            }
            else
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
        }
    }
}
