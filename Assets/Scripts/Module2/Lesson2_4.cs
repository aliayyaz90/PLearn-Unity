using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lesson2_4 : MonoBehaviour
{
    int counter;
    [SerializeField] GameObject Lesson2__4,SettingsBtn,SettingsItems;
    [SerializeField] Button[] popUpMCQ1;
    bool select1;
    [SerializeField] public GameObject[] JordanInfo;
    public bool WrongCategoryJordan, sliderWrongVal;
    [SerializeField] Slider slider;
    [SerializeField] GameObject[] MblScreens;
    [SerializeField] Text AccountInpS2,AccountInpS3, PassInp;
    [SerializeField] Toggle CheckingCheckBox;
    [SerializeField] Transform sidePopUp, sidePopUp2;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        GameManagerModule2.instance.AddMoney(0);
        GameManagerModule2.instance.AddPoint(0);
        GameManagerModule2.instance.AddBankMoney(0, 0);
        GameManagerModule2.instance.LivesCheck();
        GameManagerModule2.instance.NpcImages[0].SetActive(true);
        select1 = false;
        WrongCategoryJordan=true;
        sliderWrongVal = true;
        //GameManagerModule2.instance.player.localPosition = new Vector3(5, -6.8f, 55);
        //GameManagerModule2.instance.player.localRotation = Quaternion.Euler(0, 0, 0);
        GameManagerModule2.instance.NPCsNames.text = "Mr.Patel";
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value % 50 != 0)
        {
            slider.value /= 50;
            slider.value *= 50;
        }    
        slider.transform.GetChild(3).GetComponent<Text>().text = slider.value.ToString();
    }
    public void Task2_4NextBtn() // next button in dialog box
    {
        GameManagerModule2.instance.CurrentLevelProgress(counter, 9.0f);
        if (counter == 0 && PlayerPrefs.GetInt("patel") == 5)
        {
            GameManagerModule2.instance.DialogPanel.SetActive(true);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "Time for you to do what I did for you, help someone actually open an account, be their guiding light through this huge step in their financial journey.";
            counter++;
        }
        else if (counter == 1)
        {
            GameManagerModule2.instance.NpcImages[0].SetActive(false);
            GameManagerModule2.instance.NpcImages[2].SetActive(true);
            GameManagerModule2.instance.NPCsNames.text = "Mr.Jordan";
            GameManagerModule2.instance.DialogueText.text = "I just got my first job, and they say I need a bank account to get my paycheck every week, but I don’t even know where to start.";
           
                counter++;
        }
        else if (counter == 2)
        {
            GameManagerModule2.instance.NpcImages[0].SetActive(true);
            GameManagerModule2.instance.NpcImages[2].SetActive(false);
            GameManagerModule2.instance.NPCsNames.text = "";
            GameManagerModule2.instance.DialogueText.text = "You must help Jordan choose a student-friendly account, complete paperwork, ensuring all fields are correctly filled and make an initial deposit of $100.";
            counter++;
        }
        else if (counter == 3)
        {
            GameManagerModule2.instance.NpcImages[0].SetActive(false);
            GameManagerModule2.instance.NpcImages[2].SetActive(true);
            Lesson2__4.SetActive(true);
            if (!select1)
                GameManagerModule2.instance.Toast.Show("Select an option!", 2);
            else
                counter++;
        }
        else if (counter == 4)
        {
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            Lesson2__4.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            Lesson2__4.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        else if (counter == 5)
        {
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.NPCsNames.text = "Mr.Jordan";
            GameManagerModule2.instance.DialogueText.text = "Wow! That was quick. Now please help me set up my online banking app.";
            counter++;
        }
        else if (counter == 6)
        {
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            Lesson2__4.transform.GetChild(0).gameObject.SetActive(false);
            Lesson2__4.transform.GetChild(1).gameObject.SetActive(true);
            
        }
        else if (counter == 7)
        {
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "That was easier than I thought! Thank you so much, Alex, for helping me start my financial journey.";
            Lesson2__4.transform.GetChild(1).gameObject.SetActive(false);
            counter++;
        }
        else if (counter == 8)
        {
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.NpcImages[2].SetActive(false);
            GameManagerModule2.instance.NpcImages[0].SetActive(true);
            GameManagerModule2.instance.NPCsNames.text = "Mr.Patel";
            GameManagerModule2.instance.DialogueText.text = "I’m so proud of you Alex, for figuring all of this out so fast, now let's get you to protect your own money, because yes, the bank offers complete protection, but even they need help.";
            counter++;
        }
        else if (counter == 9)
        {
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            GameManagerModule2.instance.NpcImages[0].SetActive(false);
            PlayerPrefs.SetInt("patel", 6);
            PlayerPrefs.SetInt("gameprogress", 8);
            GameManagerModule2.instance.RewardPanell();
            GameManagerModule2.instance.AddBankMoney(50, 1);
            GameManagerModule2.instance.AddPoint(15);

        }
    }
    public void MCQ1Btn()
    {
        popUpMCQ1[1].GetComponent<Image>().color = Color.green;
        popUpMCQ1[0].interactable = false;
        popUpMCQ1[1].interactable = false;
        popUpMCQ1[2].interactable = false;
        counter++;
        select1 = true;
        GameManagerModule2.instance.PopUpShow(0);
    }

    public void MCQ1Wrong(int WrongMCQ1BtnNO)
    {
        select1 = true;
        popUpMCQ1[WrongMCQ1BtnNO].GetComponent<Image>().color = Color.red;
        if (PlayerPrefs.GetInt("walletpoint") >= 5)
        {
            GameManagerModule2.instance.Toast.Show("Points: -5", 3);
            GameManagerModule2.instance.RemovePoint(5);
        }
        if (PlayerPrefs.GetInt("lives") > 1)
            PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
        GameManagerModule2.instance.PopUpShow(1);
        if (PlayerPrefs.GetInt("lives") <= -1)
            GameManagerModule2.instance.LifeEnd();
        return;

    }

    public void JordanApplicationSubmitBtn()
    {
        if (slider.value == 100)
            sliderWrongVal = false;
        else if (slider.value != 100)
            sliderWrongVal = true;

        if (!WrongCategoryJordan && !sliderWrongVal)
        {
            counter++;
            Task2_4NextBtn();
            Lesson2__4.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
        else if (WrongCategoryJordan || sliderWrongVal)
        {
            GameManagerModule2.instance.Toast.Show("Empty or Invalid Fields", 2);
            if (PlayerPrefs.GetInt("walletpoint") >= 5)
            {
                GameManagerModule2.instance.Toast.Show("Points: -5", 3);
                GameManagerModule2.instance.RemovePoint(5);
            }
            if (PlayerPrefs.GetInt("lives") > 1)
                PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
            GameManagerModule2.instance.PopUpShow(1);
            if (PlayerPrefs.GetInt("lives") <= -1)
                GameManagerModule2.instance.LifeEnd();

            GameManagerModule2.instance.PopUpShow(5);
            return;
        }
    }
    public void ScreenBtns(int btn)
    {
        if (btn == 1)
        {
            InvokeRepeating("SidePopUpFunc",1,2.46f);
            MblScreens[1].SetActive(true);
            return;
        }
        else if (btn == 2)
        {
            if((AccountInpS2.text=="2344593321333") && PassInp.text == "2268")
            {
                MblScreens[2].SetActive(true);
                return;
            }
            else
            {
                iTween.ScaleTo(sidePopUp2.gameObject, iTween.Hash("x", 1f, "time", 1.2f, "easetype", iTween.EaseType.easeOutElastic, "loopType", "pingPong"));
                Invoke("PopHide", 2);
            }
        }
        else if (btn == 3)
        {
            if ((AccountInpS3.text == "2344593321333") && CheckingCheckBox.isOn)
            {
                CancelInvoke();
                iTween.ScaleTo(sidePopUp.gameObject, iTween.Hash("y", 0, "time", 1f));
                MblScreens[3].SetActive(true);
                return;
            }
        }
        else if (btn == 4)
        {
            
            SettingsBtn.SetActive(false);
            SettingsItems.SetActive(true);
            return;
        }
        else if (btn == 5)
        {
            if ((SettingsItems.transform.GetChild(0).GetComponent<Toggle>().isOn)&&
                (SettingsItems.transform.GetChild(1).GetComponent<Toggle>().isOn))
            {
                counter++;
                Task2_4NextBtn();
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
        return;
    }
    private void PopHide()
    {
        iTween.ScaleTo(sidePopUp2.gameObject, iTween.Hash("x", 0, "time", 1f));
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
        PlayerPrefs.SetInt("patel", 4);
        PlayerPrefs.SetInt("gameprogress", 6);
        SceneManager.LoadScene(3);
    }
}