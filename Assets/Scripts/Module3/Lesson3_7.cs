using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lesson3_7 : MonoBehaviour
{
    int counter,btnCorrectCount, sidepopCount;
    [SerializeField] GameObject Lesson3__7, Task1, Task2, Task3, sofiaListPanel;
    [SerializeField] Toggle[] task1Toggles;
    [SerializeField] Button[] task2Buttons;
    [SerializeField] Transform popUpTask2;
    [SerializeField] Transform[] ItemsTask3;

    // Start is called before the first frame update
    void Start()
    {
        sidepopCount = 0;
        btnCorrectCount = 0;
        counter = 0;
        GameManagerModule3.instance.AddMoney(0);
        GameManagerModule3.instance.AddPoint(0);
        GameManagerModule3.instance.AddBankMoney(0, 0);
        GameManagerModule3.instance.LivesCheck();
    }

    public void Task3_7NextBtn() // next button in dialog box
    {
        // Debug.Log(counter);
        GameManagerModule3.instance.CurrentLevelProgress(counter, 8.0f);
        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 7)
        {
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Okay so, I have my credit report and I have no idea what it's supposed to look like, please help me go over it and figure out what it's supposed to look like.";
            counter++;
        }
        else if (counter == 1)
        {

            Lesson3__7.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            counter++;
        }
        else if (counter == 2)
        {
            if (task1Toggles[0].isOn&& task1Toggles[1].isOn&& !task1Toggles[2].isOn && !task1Toggles[3].isOn)
            {
                GameManagerModule3.instance.PopUpShow(0);
                counter++;
                Task3_7NextBtn();
            }
            else
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
            
        }
        else if (counter == 3)
        {
            Invoke("DialogueShowWithWait",2);
            GameManagerModule3.instance.Toast.Show("+10 Points", 2);
            GameManagerModule3.instance.AddPoint(10);
            Task1.SetActive(false);
            //GameManagerModule3.instance.NpcImages[2].SetActive(true);
            //GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Alright Alex, I understand everything but I'm gonna need you to help me with a builder plan.";
            counter++;
        }
        else if (counter == 4)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task2.SetActive(true);
            InvokeRepeating("sidePopup", 1, 2);
        }
        else if (counter == 5)
        {
            Invoke("DialogueShowWithWait", 2);
            //GameManagerModule3.instance.NpcImages[2].SetActive(true);
            //GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "I heard that if a bank charges you wrong you can file something called a dispute, but I don't know how that works, can you help me, Alex?";
            Task2.SetActive(false);
            GameManagerModule3.instance.AddPoint(15);
            GameManagerModule3.instance.Toast.Show("+15 Points", 2);
            counter++;
        }
        else if (counter == 6)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task3.SetActive(true);

                counter++;
        }
        else if (counter == 7)
        {
            if (ItemsTask3[0].localPosition == new Vector3(-240, -162, 0) &&
               ItemsTask3[1].localPosition == new Vector3(-98, -213.20f, 0) &&
               ItemsTask3[2].localPosition == new Vector3(6, -265, 0) &&
               ItemsTask3[3].localPosition == new Vector3(-81.5f, -314, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                counter++;
                Task3_7NextBtn();
            }
            else
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
        }

        else if (counter == 8)
        {
            Lesson3__7.SetActive(false);
            PlayerPrefs.SetInt("sofia", 8);
            PlayerPrefs.SetInt("gameprogress", 16);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.AddPoint(20);
            GameManagerModule3.instance.AddBankMoney(100,1);
            counter++;
        }
    }

    public void SofiaListBtn(int btnNO)
    {
        if(btnNO == 0)
            sofiaListPanel.SetActive(true);
        else
            sofiaListPanel.SetActive(false);
    }
    
    public void MCQTask2(int btnNo)
    {
        if(btnNo == 0 || btnNo == 4 || btnNo == 5)
        {
            task2Buttons[btnNo].GetComponent<Image>().color = Color.green;
            GameManagerModule3.instance.PopUpShow(0);
            btnCorrectCount++;
            if (btnCorrectCount == 3)
            {
                counter++;
                CancelInvoke();
                iTween.ScaleTo(popUpTask2.gameObject, iTween.Hash("x", 0, "time", 1f));
                Task3_7NextBtn();
            }
        }
        else if(btnNo == 1 || btnNo == 2 || btnNo == 3)
        {
            task2Buttons[btnNo].GetComponent<Image>().color = Color.red;
            GameManagerModule3.instance.PopUpShow(1);
        }
    }
    public void sidePopup()
    {

        for (int i = 0; i < 3; i++)
        {
            if (i == sidepopCount)
            {
                popUpTask2.GetChild(i).gameObject.SetActive(true);
                iTween.ScaleTo(popUpTask2.gameObject, iTween.Hash("x", 1f, "time", 0.98f, "easetype", iTween.EaseType.easeOutElastic, "loopType", "pingPong"));
            }
            else
                popUpTask2.GetChild(i).gameObject.SetActive(false);
        }
        sidepopCount++;
        if (sidepopCount == 3)
            sidepopCount = 0;

    }
    public void DialogueShowWithWait()
    {
        GameManagerModule3.instance.NpcImages[2].SetActive(true);
        GameManagerModule3.instance.DialogueBox.SetActive(true);
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 7);
        PlayerPrefs.SetInt("gameprogress", 15);
        SceneManager.LoadScene(4);
    }
}
