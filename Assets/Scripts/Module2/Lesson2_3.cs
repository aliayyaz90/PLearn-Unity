using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lesson2_3 : MonoBehaviour
{
    int counter;
    [SerializeField] GameObject Lesson2__3, Task2Mcq;
    [SerializeField] Button[] popUpMCQ1, popUpMCQ2, task2popUpMCQ;
    bool select1, select2;
    [SerializeField] Toggle[] Bal1, Bal2, Bal3;
    [SerializeField] GameObject[] BankStates,states,hovers;
    [SerializeField] Transform Task2;
    int balCount,task2mcqCount;
    // Start is called before the first frame update
    void Start()
    {
        task2mcqCount = 0;
        balCount = 0;
        select1 = false;
        select2 = false;
        counter = 0;
        GameManagerModule2.instance.AddMoney(0);
        GameManagerModule2.instance.AddPoint(0);
        GameManagerModule2.instance.AddBankMoney(0,0);
        GameManagerModule2.instance.LivesCheck();
        GameManagerModule2.instance.NpcImages[1].SetActive(true);
        //GameManagerModule2.instance.player.localPosition = new Vector3(5, -6.8f, 55);
        //GameManagerModule2.instance.player.localPosition = new Vector3(45, 2.4f, 85);
        //GameManagerModule2.instance.player.localRotation = Quaternion.Euler(0, 0, 0);

        
    }
    public void Task2_3NextBtn() // next button in dialog box
    {
        //GameManagerModule2.instance.BankImg.SetActive(true);
        GameManagerModule2.instance.CurrentLevelProgress(counter, 12.0f);
        if (counter == 0 && PlayerPrefs.GetInt("patel") == 4)
        {
            GameManagerModule2.instance.DialogPanel.SetActive(true);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "I don’t trust banks, how do I know they won't just disappear with all my money, or the money won't just be robbed leaving me with nothing. If I don't even have faith in banks, why should I even open a checking account?";
            counter++;
        }
        else if (counter == 1)
        {
            GameManagerModule2.instance.DialogueText.text = "Banks charge too many fees!";
            Lesson2__3.SetActive(true);
            if (!select1)
                GameManagerModule2.instance.Toast.Show("Select an option!", 2);
            else
                counter++;
        }
        else if (counter == 2)
        {
            GameManagerModule2.instance.NPCsNames.text = "Alex";
            GameManagerModule2.instance.NpcImages[1].SetActive(false);
            GameManagerModule2.instance.NpcImages[3].SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "Banks only charge what they tell you, they charge, they let you set back and have most payments automated . The solution is to choose accounts with no fees, or lesser fees.";
            counter++;
        }
        else if (counter == 3)
        {
            GameManagerModule2.instance.NPCsNames.text = "Mr.Thompson";
            GameManagerModule2.instance.NpcImages[1].SetActive(true);
            GameManagerModule2.instance.NpcImages[3].SetActive(false);
            Lesson2__3.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            Lesson2__3.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "Cash is better.";
            counter++;
        }
        else if (counter == 4)
        {
            if (!select2)
                GameManagerModule2.instance.Toast.Show("Select an option!", 2);
            else
                counter++;
        }
        else if (counter == 5)
        {
            GameManagerModule2.instance.NPCsNames.text = "Alex";
            GameManagerModule2.instance.NpcImages[1].SetActive(false);
            GameManagerModule2.instance.NpcImages[3].SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "Cash can be lost or stolen, while bank funds are secured, not only with security, but with insurance, so even if they are stolen, the bank is liable to pay you back.";
            counter++;
        }
        else if (counter == 6)
        {
            Lesson2__3.transform.GetChild(0).gameObject.SetActive(false);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.NpcImages[3].SetActive(false);
            GameManagerModule2.instance.NpcImages[0].SetActive(true);
            GameManagerModule2.instance.NPCsNames.text = "Mr.Patel";
            GameManagerModule2.instance.DialogueText.text = "Amazing Alex! You know enough about banks and accounts now to convince someone like Thompson to open an account, you have the spirit of a true financial champion, but let's take this a step further shall we?";
            counter++;
        }
        else if (counter == 7)
        {
            GameManagerModule2.instance.DialogueText.text = "Here are 3 bank statemnts. Go over these Alex, you know enough about Banks and accounts now, to know where the bank is trying to charge you more than you owe.";
            counter++;
        }
        else if (counter == 8)
        {
            GameManagerModule2.instance.NpcImages[0].SetActive(false);
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            Task2.gameObject.SetActive(true);
            
        }
        else if (counter == 9)
        {
            //Task2.gameObject.SetActive(false);
            GameManagerModule2.instance.NpcImages[0].SetActive(true);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "Good work. Now suggest atleat 3 ways to avoid unnecessary costs.";
            counter++;
        }
        else if (counter == 10)
        {
            Task2.gameObject.SetActive(true);
            GameManagerModule2.instance.NpcImages[0].SetActive(false);
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            Task2Mcq.SetActive(true);
        }
        else if (counter == 11)
        {
            Task2.gameObject.SetActive(false);
            GameManagerModule2.instance.NpcImages[0].SetActive(true);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "A checking account, like almost every other bank service, is a tool, if you use it wisely, it can save you money instead of costing you, but you need to be very careful with where and how your money is going.";
            counter++;
        }
        else if (counter == 12)
        {
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            GameManagerModule2.instance.NpcImages[0].SetActive(false);
            PlayerPrefs.SetInt("patel", 5);
            PlayerPrefs.SetInt("gameprogress", 7);
            GameManagerModule2.instance.RewardPanell();
            GameManagerModule2.instance.AddBankMoney(200, 1);
            GameManagerModule2.instance.AddPoint(15);

        }
    }
    public void MCQ1Btn()
    {
        popUpMCQ1[2].GetComponent<Image>().color = Color.green;
        popUpMCQ1[0].interactable = false;
        popUpMCQ1[1].interactable = false;
        popUpMCQ1[2].interactable = false;
        popUpMCQ1[3].interactable = false;
        counter++;
        select1 = true;
        GameManagerModule2.instance.PopUpShow(0);
    }
    public void MCQ2Btn()
    {
        popUpMCQ2[1].GetComponent<Image>().color = Color.green;
        popUpMCQ2[2].interactable = false;
        popUpMCQ2[0].interactable = false;
        popUpMCQ2[1].interactable = false;
        popUpMCQ2[3].interactable = false;
        counter++;
        select2 = true;
        GameManagerModule2.instance.PopUpShow(0);
    }

    public void MCQ1Wrong(int WrongMCQ1BtnNO)
    {
        //select1 = true;
        popUpMCQ1[WrongMCQ1BtnNO].GetComponent<Image>().color = Color.red;
        //if (PlayerPrefs.GetInt("lives") > 1)
        //    PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
        GameManagerModule2.instance.PopUpShow(1);
        if (PlayerPrefs.GetInt("lives") <= -1)
            GameManagerModule2.instance.LifeEnd();
        return;

    }

    public void BalChecker(int BalSheetNo)
    {
        if (BalSheetNo == 1 && balCount==2)
        {
            BankStates[0].SetActive(false);
            BankStates[1].SetActive(true);
            balCount = 0;
            states[0].SetActive(true);
            states[1].SetActive(true);
            return;
        }
        else if (BalSheetNo == 2 && balCount == 2)
        {
            BankStates[1].SetActive(false);
            BankStates[2].SetActive(true);
            balCount = 0;
            states[0].SetActive(true);
            states[1].SetActive(true);
            return;
        }else if (BalSheetNo == 3 && balCount == 2)
        {
            BankStates[0].SetActive(false);
            BankStates[1].SetActive(false);
            BankStates[2].SetActive(false);
            counter++;
            Task2_3NextBtn();
            return;
        }
        //if (PlayerPrefs.GetInt("lives") > 1)
        //    PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
        GameManagerModule2.instance.PopUpShow(1);
        if (PlayerPrefs.GetInt("lives") <= -1)
            GameManagerModule2.instance.LifeEnd();
    }
    public void MCQ2Wrong(int WrongMCQ2BtnNO)
    {
       // select2 = true;
        popUpMCQ2[WrongMCQ2BtnNO].GetComponent<Image>().color = Color.red;
        //if (PlayerPrefs.GetInt("lives") > 1)
        //    PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
        GameManagerModule2.instance.PopUpShow(1);
        if (PlayerPrefs.GetInt("lives") <= -1)
            GameManagerModule2.instance.LifeEnd();
        return;

    }

    public void Task2MCQ(int mcqs)
    {
        if (mcqs == 0 || mcqs==2 || mcqs==4)
        {
            task2popUpMCQ[mcqs].GetComponent<Image>().color = Color.green;
            GameManagerModule2.instance.PopUpShow(0);
            task2mcqCount++;
            if (task2mcqCount == 3)
            {
                //Task2Mcq.SetActive(false);
                counter++;
                Task2_3NextBtn();
            }
        }
        else
        {
            task2popUpMCQ[mcqs].GetComponent<Image>().color = Color.red;
            //if (PlayerPrefs.GetInt("lives") > 1)
            //    PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
            GameManagerModule2.instance.PopUpShow(1);
            if (PlayerPrefs.GetInt("lives") <= -1)
                GameManagerModule2.instance.LifeEnd();
            return;
        }
    }
    public void BankStateChecker(int btnNo)
    {
        
            if (btnNo ==0 || btnNo == 1)
            {
                states[btnNo].SetActive(false);
                hovers[btnNo].SetActive(false);
                balCount++;
            return;
            }
        //if (PlayerPrefs.GetInt("lives") > 1)
        //    PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
        GameManagerModule2.instance.PopUpShow(1);
        if (PlayerPrefs.GetInt("lives") <= -1)
            GameManagerModule2.instance.LifeEnd();
        return;

    }

    //public void ShowHoverObj()
    //{
    //    HoverGameObj.SetActive(true);
    //}
    //public void HideHoverObj()
    //{
    //    HoverGameObj.SetActive(false);
    //}
    public void Retry()
    {
        PlayerPrefs.SetInt("patel", 4);
        PlayerPrefs.SetInt("gameprogress", 6);
        SceneManager.LoadScene(3);
    }
    
}
