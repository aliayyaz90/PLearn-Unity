using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ToastWrapper;


public class Lesson3_1 : MonoBehaviour
{
    int counter;
    [SerializeField] GameObject Lesson3__1, CreditCard, Task1,Task3;
    [SerializeField] Button[] popUpMCQ1;
    bool select1;
    [SerializeField] GameObject[] Items, Characters, Characters2, CreditChecks;
    [SerializeField] Transform Task2;
    int characterCount, defaultCount;
    [SerializeField] Toggle[] toggles;
  
    // Start is called before the first frame update
    void Start()
    {
        defaultCount = 0;
        characterCount = 0;
        select1 = false;
        counter = 0;
        GameManagerModule3.instance.AddMoney(0);
        GameManagerModule3.instance.AddPoint(0);
        GameManagerModule3.instance.AddBankMoney(0, 0);
        GameManagerModule3.instance.LivesCheck();
        GameManagerModule3.instance.NPCsNames.text = "Sofia";
        GameManagerModule3.instance.NpcImages[2].SetActive(true);
        GameManagerModule3.instance.NpcImages[1].SetActive(false);


    }
    public void Task3_1NextBtn() // next button in dialog box
    {
        //GameManagerModule3.instance.BankImg.SetActive(true);
        GameManagerModule3.instance.CurrentLevelProgress(counter, 13.0f);
        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 2)
        {
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Welcome to Zurich Alex, I heard about the Amazing work you did in Belfast and Paris, and I hope you can help me the same way here too.";
            counter++;
        }
        else if (counter == 1)
        {
            GameManagerModule3.instance.DialogueText.text = "Alex’ I keep hearing people talk about something called Credit, and how they can get money through it, but I have no idea how it works, can you help me out?";
            counter++;
        }
        else if (counter == 2)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueText.text = " Choose the correct definition of credit based on the analogy";
            Lesson3__1.SetActive(true);
            if (!select1)
                GameManagerModule3.instance.Toast.Show("Select an option!", 2);
            else
                counter++;
        }
        else if (counter == 3)
        {
            GameManagerModule3.instance.NPCsNames.text = "Alex";
            GameManagerModule3.instance.NpcImages[1].SetActive(false);
            GameManagerModule3.instance.NpcImages[3].SetActive(true);
            Lesson3__1.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            //Task1.SetActive(false);
            GameManagerModule3.instance.DialogueText.text = "A neighbor buys a lawnmower in installments and slowly pays it off.";
            counter++;
        }
        else if (counter == 4)
        {
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            //GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.NpcImages[3].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Invoke("DialogueShowWithWait", 1);
            GameManagerModule3.instance.DialogueText.text = "Thankyou Alex for helping me out.";
            GameManagerModule3.instance.Toast.Show("+50 Points", 1f);
            GameManagerModule3.instance.AddPoint(50);
            CreditCard.SetActive(true);
            counter++;
        }
        else if (counter == 5)
        {
            CreditCard.SetActive(false);
            GameManagerModule3.instance.DialogueText.text = "Alex I signed you up for a market based on lending and borrowing. You'll learn a lot from this, you'll have to lend and borrow things here, and we'll see if you can come out with a profit?";
            counter++;
        }
        else if (counter == 6)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task1.SetActive(false);
            Task2.gameObject.SetActive(true);
            if ((Characters[characterCount].activeInHierarchy && Items[characterCount].transform.localPosition== new Vector3(105,-302,0)) ||
                (Characters[characterCount].activeInHierarchy && Items[characterCount].transform.localPosition == new Vector3(105, -302, 0)))
            { 
                if (characterCount <= 1)
                {
                    Items[characterCount].SetActive(false);
                    Items[characterCount].transform.localPosition = new Vector3(0, 0, 0);
                    Characters[characterCount].SetActive(false);
                    characterCount++;
                    if(characterCount<2)
                        Characters[characterCount].SetActive(true);

                    GameManagerModule3.instance.PopUpShow(0);
                }
                if(characterCount == 2)
                {
                    counter++;
                    Task3_1NextBtn();
                }
            }
            else if(defaultCount>0)
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
                return;
            }
            defaultCount++;
        }
        else if (counter == 7)
        {
            Task2.GetChild(0).gameObject.SetActive(false);
            Task2.GetChild(1).gameObject.SetActive(true);
        }
        else if (counter == 8)
        {
            //GameManagerModule3.instance.DialogueBox.SetActive(true);
            //GameManagerModule3.instance.NpcImages[2].SetActive(true);
            Invoke("DialogueShowWithWait", 2);
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            GameManagerModule3.instance.DialogueText.text = "Amazing Alex! You made a profit.";
            GameManagerModule3.instance.Toast.Show("+10 Points\n+100 Money",2);
            GameManagerModule3.instance.AddPoint(10);
            GameManagerModule3.instance.AddBankMoney(100,1);
            counter++;
        }
        else if (counter == 9)
        {
            GameManagerModule3.instance.DialogueText.text = "I'm sorry Alex, but it feels impossible to understand how credit really works, if you could just help me pick out of these?";
            counter++;
        }
        else if (counter == 10)
        {
            GameManagerModule3.instance.DialogueText.text = "Check reasons on a list as to what needs to be involved for something to be considered credit.";
            counter++;
           

        }
        else if (counter == 11)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task3.gameObject.SetActive(true);
        }
        else if (counter == 12)
        {
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "A checking account, like almost every other bank service, is a tool, if you use it wisely, it can save you money instead of costing you, but you need to be very careful with where and how your money is going.";
            counter++;
        }
        else if (counter == 13)
        {
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            GameManagerModule3.instance.NpcImages[0].SetActive(false);
            PlayerPrefs.SetInt("sofia", 1);
            PlayerPrefs.SetInt("gameprogress", 10);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.AddBankMoney(100, 1);
            GameManagerModule3.instance.AddPoint(10);

        }
    }
    public void MCQ1Btn()
    {
        popUpMCQ1[1].GetComponent<Image>().color = Color.green;
        popUpMCQ1[0].interactable = false;
        popUpMCQ1[1].interactable = false;
        popUpMCQ1[2].interactable = false;
        popUpMCQ1[3].interactable = false;
        counter++;
        select1 = true;
        GameManagerModule3.instance.PopUpShow(0);
    }
    public void DialogueShowWithWait()
    {
        GameManagerModule3.instance.NpcImages[2].SetActive(true);
        GameManagerModule3.instance.DialogueBox.SetActive(true);
    }
    public void OkBtn()
    {
        if (Characters2[0].activeInHierarchy)
        {
            Characters2[1].SetActive(true);
            Characters2[0].SetActive(false);
            GameManagerModule3.instance.PopUpShow(0);
        }
        else
        {
            GameManagerModule3.instance.PopUpShow(1);
            if (PlayerPrefs.GetInt("lives") <= -1)
                GameManagerModule3.instance.LifeEnd();
        }
            
    }
    public void FineBtn()
    {
        if (Characters2[1].activeInHierarchy)
        {
            GameManagerModule3.instance.PopUpShow(0);
            Task2.gameObject.SetActive(false);
            counter++;
            Task3_1NextBtn();
        }
        else
        {
            GameManagerModule3.instance.PopUpShow(1);
            if (PlayerPrefs.GetInt("lives") <= -1)
                GameManagerModule3.instance.LifeEnd();
        }

    }
    public void MCQ1Wrong(int WrongMCQ1BtnNO)
    {
        popUpMCQ1[WrongMCQ1BtnNO].GetComponent<Image>().color = Color.red;
        GameManagerModule3.instance.PopUpShow(1);
        if (PlayerPrefs.GetInt("lives") <= -1)
            GameManagerModule3.instance.LifeEnd();
        return;

    }

    public void CreditUse()
    {
        if (CreditChecks[0].activeInHierarchy && toggles[0].isOn && toggles[1].isOn && !toggles[2].isOn)
        {
            CreditChecks[0].SetActive(false);
            GameManagerModule3.instance.PopUpShow(0);
            CreditChecks[1].SetActive(true);
        }
        else if(CreditChecks[1].activeInHierarchy && toggles[4].isOn && toggles[5].isOn && !toggles[3].isOn)
        {
            CreditChecks[1].SetActive(false);
            GameManagerModule3.instance.PopUpShow(0);
            CreditChecks[2].SetActive(true);
        }
        else if (CreditChecks[2].activeInHierarchy && toggles[6].isOn && toggles[8].isOn && !toggles[7].isOn)
        {
            CreditChecks[2].SetActive(false);
            GameManagerModule3.instance.PopUpShow(0);
            CreditChecks[3].SetActive(true);
        }
        else if (CreditChecks[3].activeInHierarchy && toggles[9].isOn && toggles[10].isOn && !toggles[11].isOn)
        {
            CreditChecks[3].SetActive(false);
            GameManagerModule3.instance.PopUpShow(0);
            CreditChecks[4].SetActive(true);
        }
        else if (CreditChecks[4].activeInHierarchy && toggles[12].isOn && toggles[14].isOn && !toggles[13].isOn)
        {
            GameManagerModule3.instance.PopUpShow(0);
            Task3.SetActive(false);
            counter++;
            Task3_1NextBtn();
        }
        else
        {
            GameManagerModule3.instance.PopUpShow(1);
            if (PlayerPrefs.GetInt("lives") <= -1)
                GameManagerModule3.instance.LifeEnd();
        }
    }

    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 0);
        PlayerPrefs.SetInt("gameprogress", 9);
        SceneManager.LoadScene(4);
    }
}
