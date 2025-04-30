using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lesson3_10 : MonoBehaviour
{
    int counter, total1,total2;
    float interest1,interest2;
    [SerializeField] GameObject Lesson3__10, Task1, Task2;
    [SerializeField] Slider[] task1Sliders;
    [SerializeField] Text Amount1, Amount2;
    //[SerializeField] Button[] task2Btns;


    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        GameManagerModule3.instance.AddMoney(0);
        GameManagerModule3.instance.AddPoint(0);
        GameManagerModule3.instance.AddBankMoney(0, 0);
        GameManagerModule3.instance.LivesCheck();
    }
    private void Update()
    {
        if (task1Sliders[0].value % 10000 != 0)
            task1Sliders[0].value = task1Sliders[0].value - (task1Sliders[0].value % 10000);
        
        task1Sliders[0].transform.GetChild(4).GetComponent<Text>().text = task1Sliders[0].value.ToString();
        task1Sliders[1].transform.GetChild(4).GetComponent<Text>().text = task1Sliders[1].value.ToString();
        task1Sliders[2].transform.GetChild(4).GetComponent<Text>().text = task1Sliders[2].value.ToString();
        task1Sliders[3].transform.GetChild(4).GetComponent<Text>().text = task1Sliders[3].value.ToString();

        interest1=  task1Sliders[1].value / 100;
        interest2=  task1Sliders[2].value / 100;

        total1 = (Int32)(task1Sliders[0].value * (1+(interest1 * task1Sliders[3].value)));
        Amount1.text = total1.ToString();
        total2 = (Int32)(task1Sliders[0].value * (1 + (interest2 * task1Sliders[3].value)));
        Amount2.text = total2.ToString();
        //Debug.Log("perMonth: " + perMonth + "  interest: " + interest + "  repayment: "+ task1Sliders[3].value);
    }
    public void Task3_10NextBtn() // next button in dialog box
    {
        // Debug.Log(counter);
        GameManagerModule3.instance.CurrentLevelProgress(counter, 5.0f);
        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 10)
        {
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Okay Alex, this time I need you to help me figure out how this loan repayment works.";
            counter++;
        }
        else if (counter == 1)
        {

            Lesson3__10.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            counter++;
        }
        else if (counter == 2)
        {
            Task1.SetActive(false);
            GameManagerModule3.instance.Toast.Show("+10 Points\nMoney saved for Sofia", 2);
            Invoke("DialogueShowWithWait", 2);
            GameManagerModule3.instance.DialogueText.text = "Alright, I have managed to get myself three different loan offers, but I don't know which one's best so can you please help me out.";
            counter++;
        }
        else if (counter == 3)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task2.SetActive(true);
            counter++;
        }
        else if (counter == 4)
        {
            Task2.transform.GetChild(0).gameObject.SetActive(false);
            Task2.transform.GetChild(1).gameObject.SetActive(true);
            Task2.transform.GetChild(2).gameObject.SetActive(true);
        }

        else if (counter == 5)
        {
            Lesson3__10.SetActive(false);
            PlayerPrefs.SetInt("sofia", 11);
            PlayerPrefs.SetInt("gameprogress", 19);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.AddBankMoney(100, 1);
            GameManagerModule3.instance.AddPoint(20);
            counter++;
        }
    }
    public void ShowLoanBtn()
    {
        if (!Task2.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            Task2.transform.GetChild(1).gameObject.SetActive(false);
            Task2.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            Task2.transform.GetChild(1).gameObject.SetActive(true);
            Task2.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void task2Btnss(int btnNo)
    {
        if(btnNo == 0)
        {
            GameManagerModule3.instance.PopUpShow(0);
            counter++;
            Task3_10NextBtn();
        }
        else
        {
            GameManagerModule3.instance.PopUpShow(1);
            if (PlayerPrefs.GetInt("lives") <= -1)
                GameManagerModule3.instance.LifeEnd();
        }
    }
    public void DialogueShowWithWait()
    {
        GameManagerModule3.instance.NpcImages[2].SetActive(true);
        GameManagerModule3.instance.DialogueBox.SetActive(true);
    }

    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 10);
        PlayerPrefs.SetInt("gameprogress", 18);
        SceneManager.LoadScene(4);
    }
}
