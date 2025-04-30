using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Lesson3_4 : MonoBehaviour
{
    int counter,sidepopCount, MCQNo;
    float timer;
    [SerializeField] GameObject Lesson3__4, Task1, Task2;
    [SerializeField] Transform sidePopUp, task2Popup;
    string[] headings = { "Student Travel", "Debt Transfer", "Online Shopping", "Car loan", "House Rent" };
    //                       credit card       apr balance      bnpl              apr         debit card

    [SerializeField] Button[] popUpMCQ2;
    [SerializeField] Text headingText,TimerText;
    // Start is called before the first frame update
    void Start()
    {
        task2Popup.GetChild(0).GetComponent<Text>().text = "You couldn't select correct method in time";
        timer = 20f;
        MCQNo = 0;
        sidepopCount = 0;
        counter = 0;
        GameManagerModule3.instance.AddMoney(0);
        GameManagerModule3.instance.AddPoint(0);
        GameManagerModule3.instance.AddBankMoney(0, 0);
        GameManagerModule3.instance.LivesCheck();
        GameManagerModule3.instance.NPCsNames.text = "Sofia";
        GameManagerModule3.instance.NpcImages[2].SetActive(true);


    }

    private void Update()
    {
        if (counter == 4)
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
                TimerText.text = "Timer " + ((Int32)timer).ToString();
                if (timer <= 0)
                {
                    iTween.ScaleTo(task2Popup.gameObject, iTween.Hash("y", 1f, "time", 0.98f, "easetype", iTween.EaseType.easeOutElastic));
                    Invoke("Retry", 2);
                    if (PlayerPrefs.GetInt("lives") <= -1)
                        GameManagerModule3.instance.LifeEnd();
                }

            }
        }
    }
    public void Task3_4NextBtn() // next button in dialog box
    {
        GameManagerModule3.instance.CurrentLevelProgress(counter, 5.0f);
        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 4)
        {
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Help me go over these, Alex. Please, I need to pick the best option, but I can't even wrap my head around what these mean.";
            counter++;
        }
        else if (counter == 1)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            Lesson3__4.SetActive(true);
            InvokeRepeating("sidePopup", 1, 2);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            counter++;
        }
        else if (counter == 2)
        {
            CancelInvoke();
            iTween.ScaleTo(sidePopUp.gameObject, iTween.Hash("x", 0, "time", 1f));
            //Task2.SetActive(true);
            Task1.SetActive(false);
            GameManagerModule3.instance.PopUpShow(0);
            //GameManagerModule3.instance.NpcImages[1].SetActive(false);
            //GameManagerModule3.instance.DialogueBox.SetActive(false);
            GameManagerModule3.instance.AddBankMoney(100, 1);
            GameManagerModule3.instance.AddPoint(15);
            GameManagerModule3.instance.Toast.Show("+15 Points\n+100 Money", 2);
            //GameManagerModule3.instance.DialogPanel.SetActive(true);
            //GameManagerModule3.instance.DialogueBox.SetActive(true);
            Invoke("DialogueShowWithWait", 2f);
            GameManagerModule3.instance.DialogueText.text = "Alex! There is so much to do, and it's all piled up, please just gets get through this as fast as we can!";
            counter++;
        }
        else if (counter == 3)
        {
            //GameManagerModule3.instance.DialogPanel.SetActive(true);
            //GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "I have to pay for my student trip, transfer debt, and shop online. Help me pick the best card or method—fast!";
            //GameManagerModule3.instance.AddBankMoney(100, 1);
            //GameManagerModule3.instance.AddPoint(15);
            //GameManagerModule3.instance.Toast.Show("+15 Points\n+100 Money", 2);
            //CancelInvoke();
            //iTween.ScaleTo(sidePopUp.gameObject, iTween.Hash("x", 0, "time", 1f));
            counter++;
        }
        else if (counter == 4)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task1.SetActive(false);
            Task2.SetActive(true);
            headingText.text = headings[0];
            //counter++;
            if (MCQNo < 4)
                GameManagerModule3.instance.Toast.Show("Select an option", 2);
        }
        else if (counter == 5)
        {
            CancelInvoke();
            Lesson3__4.SetActive(false);
            PlayerPrefs.SetInt("sofia", 5);
            PlayerPrefs.SetInt("gameprogress", 13);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.AddBankMoney(100, 1);
            GameManagerModule3.instance.AddPoint(15);
        }
    }
    public void Task1WrongBtn()
    {
        GameManagerModule3.instance.PopUpShow(1);
        if (PlayerPrefs.GetInt("lives") <= -1)
            GameManagerModule3.instance.LifeEnd();
    }
    public void sidePopup()
    {

            for (int i = 0; i < 3; i++)
            {
                if (i == sidepopCount)
                {
                    sidePopUp.GetChild(i).gameObject.SetActive(true);
                    iTween.ScaleTo(sidePopUp.gameObject, iTween.Hash("x", 1f, "time", 0.98f, "easetype", iTween.EaseType.easeOutElastic, "loopType", "pingPong"));
                }
                else
                    sidePopUp.GetChild(i).gameObject.SetActive(false);
            }
            sidepopCount++;
            if (sidepopCount == 3)
                sidepopCount = 0;
       
    }

    public void Task2MCQs(int BtnNo)
    {
        if(MCQNo==0 && BtnNo == 0)
        {
           popUpMCQ2[BtnNo].GetComponent<Image>().color = Color.green;
            GameManagerModule3.instance.PopUpShow(0);
            MCQNo=1;
            headingText.text = headings[1];
            timer = 20f;
        }
        else if (MCQNo == 1 && BtnNo == 1)
        {
            popUpMCQ2[BtnNo].GetComponent<Image>().color = Color.green;
            GameManagerModule3.instance.PopUpShow(0);
            MCQNo = 2;
            headingText.text = headings[2];
            timer = 20f;
        }
        else if (MCQNo == 2 && BtnNo == 2)
        {
            popUpMCQ2[BtnNo].GetComponent<Image>().color = Color.green;
            GameManagerModule3.instance.PopUpShow(0);
            MCQNo = 3;
            headingText.text = headings[3];
            timer = 20f;
        }
        else if (MCQNo == 3 && BtnNo == 1)
        {
            popUpMCQ2[BtnNo].GetComponent<Image>().color = Color.green;
            GameManagerModule3.instance.PopUpShow(0);
            MCQNo = 4;
            headingText.text = headings[4];
            timer = 20f;          
        }
        else if (MCQNo == 4 && BtnNo == 3)
        {
            popUpMCQ2[BtnNo].GetComponent<Image>().color = Color.green;
            GameManagerModule3.instance.PopUpShow(0);
            counter++;
            Task3_4NextBtn();
        }
        else
        {
            GameManagerModule3.instance.PopUpShow(1);
            if (PlayerPrefs.GetInt("lives") <= -1)
                GameManagerModule3.instance.LifeEnd();
        }
        Invoke("BtnColor", 1);
    }
    public void BtnColor()
    {
        popUpMCQ2[0].GetComponent<Image>().color = Color.white;
        popUpMCQ2[1].GetComponent<Image>().color = Color.white;
        popUpMCQ2[2].GetComponent<Image>().color = Color.white;
        popUpMCQ2[3].GetComponent<Image>().color = Color.white;
    }
    public void DialogueShowWithWait()
    {
        GameManagerModule3.instance.NpcImages[2].SetActive(true);
        GameManagerModule3.instance.DialogueBox.SetActive(true);
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 4);
        PlayerPrefs.SetInt("gameprogress", 12);
        SceneManager.LoadScene(4);
    }
}
