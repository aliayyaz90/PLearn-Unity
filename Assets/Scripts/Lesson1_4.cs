using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Lesson1_4 : MonoBehaviour
{
    int counter;
    int CorrentNum;
    public Slider[] sliders;
    [SerializeField] Transform Lesson1_4UI;
    public static bool AnimationLevel = false;
    void Start()
    {
        //AnimationLevel = true;
        GameManager.instance.wallet.SetActive(true);
        GameManager.instance.LivesCheck();
        counter = 0;
        GameManager.instance.AddMoney(0);
        GameManager.instance.AddPoint(0);
        GameManager.instance.Toast.Show("Go back to Grandma Millie with cake", 3);
       // Debug.Log(GameManager.instance.player.localPosition);
        GameManager.instance.NpcImages[0].SetActive(true);
    }
    private void Update()
    {
        if (sliders[0].value % 10 != 0 || sliders[1].value % 10 != 0 || sliders[2].value % 10 != 0)
        {
            sliders[0].value /= 10;
            sliders[0].value *= 10;
            sliders[1].value /= 10;
            sliders[1].value *= 10;
            sliders[2].value /= 10;
            sliders[2].value *= 10;
            sliders[0].transform.GetChild(4).GetComponent<Text>().text = sliders[0].value.ToString() + "%";
            sliders[1].transform.GetChild(4).GetComponent<Text>().text = sliders[1].value.ToString() + "%";
            sliders[2].transform.GetChild(4).GetComponent<Text>().text = sliders[2].value.ToString() + "%";
        }
        if (sliders[0].value + sliders[1].value + sliders[2].value > 100)
        {
            if (sliders[0].value > 50)
                sliders[0].value -= 10;
            else if(sliders[1].value > 30)
                sliders[1].value -= 10;
            else if(sliders[2].value>20)
                sliders[2].value -= 10;
            sliders[0].transform.GetChild(4).GetComponent<Text>().text = sliders[0].value.ToString() + "%";
            sliders[1].transform.GetChild(4).GetComponent<Text>().text = sliders[1].value.ToString() + "%";
            sliders[2].transform.GetChild(4).GetComponent<Text>().text = sliders[2].value.ToString() + "%";
        }
    }
    public void Task4NextBtn() // next button in dialog box
    {
        GameManager.instance.CurrentLevelProgress(counter, 8.0f);
        if (counter == 0 && PlayerPrefs.GetInt("grandma") == 5)
        {
            
            GameManager.instance.DialogPanel.SetActive(true);
            GameManager.instance.DialogueBox.SetActive(true);
            GameManager.instance.DialogueText.text = "Thankyou Alex for bringing cakes. Now lets discuss the importance of budgeting for personal finances.";
            counter++;
        }
        else if (counter == 1)
        {
            GameManager.instance.DialogueText.text = "Spending on business is all fine Alex, but it's the small things, the personal expenses that take you out.";
            counter++;
        }
        else if (counter == 2)
        {
            GameManager.instance.DialogueBox.SetActive(false);
            Lesson1_4UI.gameObject.SetActive(true);
            Lesson1_4UI.GetChild(0).gameObject.SetActive(true);
            GameManager.instance.DialogPanel.transform.GetChild(1).gameObject.SetActive(true);
            counter++;
        }
        else if (counter == 3)
        {
            GameManager.instance.DialogPanel.transform.GetChild(1).gameObject.SetActive(false);
            GameManager.instance.DialogueBox.SetActive(true);
            Lesson1_4UI.GetChild(0).gameObject.SetActive(false);
            GameManager.instance.DialogueText.text = "See, Alex, a budget isn't about restriction; it's about making sure you always have enough for what truly matters, making sure you don't end up out of money. Let’s create one together!";
            counter++;
        }
        else if (counter == 4)
        {
            GameManager.instance.DialogueText.text = "Now I will give 2000 and you have to allocate the right amount according to the chart";
            counter++;
        }
        else if (counter == 5)
        {
            GameManager.instance.DialogPanel.transform.GetChild(1).gameObject.SetActive(true);
            GameManager.instance.DialogueBox.SetActive(false);
            Lesson1_4UI.GetChild(1).gameObject.SetActive(true);
            sliders[0].transform.parent.gameObject.SetActive(true);
            GameManager.instance.nextButton.interactable = true;
            counter++;
        }
        else if (counter == 6)
        {    
            if (!(sliders[0].value==50 &&  sliders[1].value==30 && sliders[2].value == 20))
            {
                
                //GameManager.instance.DialogueBox.SetActive(true);
               
                //GameManager.instance.DialogueText.text = "You didn't allocated the amount according to the chart. Try again.";
             
                GameManager.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManager.instance.LifeEnd();
                if (PlayerPrefs.GetInt("walletpoint") >= 5)
                {
                    GameManager.instance.Toast.Show("Points: -5", 3);
                    GameManager.instance.RemovePoint(5);
                }
            }
            else
            {
                sliders[0].transform.GetChild(5).gameObject.SetActive(true);
                sliders[1].transform.GetChild(5).gameObject.SetActive(true);
                sliders[2].transform.GetChild(5).gameObject.SetActive(true);
                sliders[0].interactable = false;
                sliders[1].interactable = false;
                sliders[2].interactable = false;
                GameManager.instance.PopUpShow(0);
            }
            counter++;
        }
        else if (counter == 7)
        {
            GameManager.instance.DialogPanel.transform.GetChild(1).gameObject.SetActive(false);
            GameManager.instance.DialogueBox.SetActive(true);
            Lesson1_4UI.GetChild(1).gameObject.SetActive(false);
            sliders[0].transform.parent.gameObject.SetActive(false);
            GameManager.instance.DialogueText.text = "You see, Alex, when you follow that rule, your income is spent wisely; it gets complicated when money grows, and taxes come into play but follow the 50/30/20 rule, and you'll be fine.";
            counter++;
        }
        else if (counter == 8)
        {
            GameManager.instance.DialogPanel.SetActive(false);
            PlayerPrefs.SetInt("gameprogress", 4);
            PlayerPrefs.SetInt("grandma", 6);
            AnimationLevel = true;
            GameManager.instance.RewardPanell();
            GameManager.instance.AddMoney(450);
            GameManager.instance.AddPoint(25);
            PlayerPrefs.SetInt("modules", 1);
            //PlayerPrefs.SetInt("warn", 0);
        }   
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("grandma", 4);
        PlayerPrefs.SetInt("gameprogress", 3);
        SceneManager.LoadScene(2);
    }
}