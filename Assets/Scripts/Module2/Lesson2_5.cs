using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lesson2_5 : MonoBehaviour
{
    int counter;
    [SerializeField] GameObject Lesson2__5, SettingsBtn, SettingsItems;
    [SerializeField] Transform Task1, Task2, sidePopUp,sidePopUp2;
    [SerializeField] Toggle[] Email1Tog, Email2Tog, Email3Tog;
    bool Email1Check,Email2Check,Email3Check;
    [SerializeField] GameObject[] MblScreens;
    [SerializeField]
    Text AccountInpS2, NewPassText, PassInp;
    // Start is called before the first frame update
    void Start()
    {
        Email1Check = false;
        Email2Check = false;
        Email3Check = false;
        counter = 0;
        GameManagerModule2.instance.AddMoney(0);
        GameManagerModule2.instance.AddPoint(0);
        GameManagerModule2.instance.AddBankMoney(0, 0);
        GameManagerModule2.instance.LivesCheck();
        GameManagerModule2.instance.NpcImages[0].SetActive(true);
        GameManagerModule2.instance.NPCsNames.text = "Mr.Patel";
    }

    public void Task2_5NextBtn() // next button in dialog box
    {
        GameManagerModule2.instance.CurrentLevelProgress(counter, 7.0f);
        if (counter == 0 && PlayerPrefs.GetInt("patel") == 6)
        {
            GameManagerModule2.instance.DialogPanel.SetActive(true);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "Belfast residents are receiving phishing emails. People need to know what the bank will, and will not ask of them, and how they can make sure they need their account safe and secure.";
            counter++;
        }
        else if (counter == 1)
        {
            GameManagerModule2.instance.DialogueText.text = "Analyze these three emails claiming to be from the bank.";
            counter++;
        }
        else if (counter == 2)
        {
            GameManagerModule2.instance.NpcImages[0].SetActive(false);
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            Lesson2__5.SetActive(true);
        }
        else if (counter == 3)
        {
            GameManagerModule2.instance.NpcImages[0].SetActive(true);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            Task1.gameObject.SetActive(false);
            GameManagerModule2.instance.DialogueText.text = "Good, you know how to see and perceive the threat, now, let's see if you can prevent it?";
            counter++;

        }
        else if (counter == 4)
        {
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            Task2.gameObject.SetActive(true);
        }
        else if (counter == 5)
        {
            Task2.gameObject.SetActive(false);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "WELL DONE ALEX! You did a great job. Financial security is just as important as financial growth, now you know how to work with banks, and keep your money safe. Protect your money like you protect your home Alex, cuz money is how you get a home.";
            counter++;
        }
        else if (counter == 6)
        {
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "You’re ready for the next step in your financial journey! With a checking account in place, it’s time to learn about credit and loans, which are powerful tools when used wisely.";
            counter++;
        }
        else if (counter == 7)
        {
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            GameManagerModule2.instance.NpcImages[0].SetActive(false);
            PlayerPrefs.SetInt("patel", 7);
            PlayerPrefs.SetInt("gameprogress", 9);
            PlayerPrefs.SetInt("modules", 2);
            GameManagerModule2.instance.RewardPanell();
            GameManagerModule2.instance.AddBankMoney(100, 1);
            GameManagerModule2.instance.AddPoint(20);

        }
    }

    public void EmailChecker(int EmailNo)
    {
        if (EmailNo == 1)
        {
            Email1Check = false;
            if (Email1Tog[0].isOn && Email1Tog[1].isOn)
                Email1Check = true;
            for (int i = 0; i < Email1Tog.Length; i++)
            {
                if (i != 0 && i != 1)
                {
                    if (Email1Tog[i].isOn)
                        Email1Check = false;
                }
            }
            if (Email1Check)
            {
                Task1.GetChild(0).gameObject.SetActive(false);
                Task1.GetChild(1).gameObject.SetActive(true);
                return;
            }
        }
        else if (EmailNo == 2)
        {
            Email2Check = false;
            if (Email2Tog[1].isOn &&Email2Tog[2].isOn)
                Email2Check = true;
            for (int i = 0; i < Email2Tog.Length; i++)
            {
                if (i != 1 && i != 2)
                {
                    if (Email2Tog[i].isOn)
                        Email2Check = false;
                }
            }
            if (Email2Check)
            {
                Task1.GetChild(1).gameObject.SetActive(false);
                Task1.GetChild(2).gameObject.SetActive(true);
                return;
            }
        }
        else if (EmailNo == 3)
        {
            Email3Check = false;
            if (Email3Tog[1].isOn)
                Email3Check = true;
            for (int i = 0; i < Email3Tog.Length; i++)
            {
                if (i != 1)
                {
                    if (Email3Tog[i].isOn)
                        Email3Check = false;
                }
            }
            if (Email3Check)
            {
                counter++;
                Task2_5NextBtn();
                return;
            }
        }
        GameManagerModule2.instance.PopUpShow(1);
        if (PlayerPrefs.GetInt("lives") <= -1)
            GameManagerModule2.instance.LifeEnd();
    }
    public void ScreenBtns2_5(int btn)
    {
        if (btn == 1)
        {
            InvokeRepeating("SidePopUpFunc", 1, 2.46f);
            MblScreens[1].SetActive(true);
            return;
        }
        else if (btn == 2)
        {
            if ((AccountInpS2.text == "2344593321333") && PassInp.text == "2268")
            {
                CancelInvoke();
                MblScreens[2].SetActive(true);
                iTween.ScaleTo(sidePopUp.gameObject, iTween.Hash("y", 0, "time", 1f));
                return;
            }
        }
        else if (btn == 3)
        {
            SettingsBtn.SetActive(false);
            SettingsItems.SetActive(true);
            return;
        }
        else if (btn == 4)
        {
            if (SettingsItems.transform.GetChild(0).GetComponent<Toggle>().isOn &&
                SettingsItems.transform.GetChild(1).GetComponent<Toggle>().isOn &&
                ( NewPassText.text!="" && NewPassText.text!="2268"))
            {
                counter++;
                Task2_5NextBtn();
                return;
            }
        }
        if (PlayerPrefs.GetInt("walletpoint") >= 5)
        {
            GameManagerModule2.instance.Toast.Show("Points: -5", 3);
            GameManagerModule2.instance.RemovePoint(5);
        }
        GameManagerModule2.instance.PopUpShow(1);
        if (PlayerPrefs.GetInt("lives") <= -1)
            GameManagerModule2.instance.LifeEnd();

        iTween.ScaleTo(sidePopUp2.gameObject, iTween.Hash("x", 1f, "time", 1.2f, "easetype", iTween.EaseType.easeOutElastic, "loopType", "pingPong"));
        Invoke("PopHide", 2);
        return;
    }
    private void PopHide()
    {
        iTween.ScaleTo(sidePopUp2.gameObject, iTween.Hash("x", 0,"time" , 1f));
    }
    public void SidePopUpFunc()
    {
        if (sidePopUp.GetChild(0).gameObject.activeInHierarchy)
        {
            sidePopUp.GetChild(0).gameObject.SetActive(false);
            sidePopUp.GetChild(1).gameObject.SetActive(true);
        }
        else if (sidePopUp.GetChild(1).gameObject.activeInHierarchy)
        {
            sidePopUp.GetChild(1).gameObject.SetActive(false);
            sidePopUp.GetChild(0).gameObject.SetActive(true);
        }
        iTween.ScaleTo(sidePopUp.gameObject, iTween.Hash("y", 1f, "time", 1.2f, "easetype", iTween.EaseType.easeOutElastic, "loopType", "pingPong"));
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("patel", 6);
        PlayerPrefs.SetInt("gameprogress", 8);
        SceneManager.LoadScene(3);
    }
}