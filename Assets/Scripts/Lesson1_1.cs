using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Lesson1_1 : MonoBehaviour
{
    [SerializeField] public GameObject[] ExpensesArr;
    int counter;
    public int expenceItem;
    int[] ExpensesIdeal = { 250, 100, 150, 300 };
    int moneycheck,sliderval;
    public static bool wrongCatagory;
    public GameObject Lesson1_1UI;   
    void Start()
    {       
        wrongCatagory = true;
        counter = 0;
        expenceItem = -1;
        GameManager.instance.LivesCheck();
        if (PlayerPrefs.GetInt("grandma") == 1)
        {
            GameManager.instance.wallet.SetActive(true);
            GameManager.instance.AddMoney(0);
            GameManager.instance.AddPoint(0);
        }
        GameManager.instance.player.localPosition = new Vector3(5, -6.8f, 55);
        GameManager.instance.player.localRotation=Quaternion.Euler(0, 0, 0);
        GameManager.instance.NpcImages[0].SetActive(true);
        
    }
    void Update()
    {
        if (GameManager.instance.slider1.value % 50 != 0)
        {
            GameManager.instance.slider1.value /= 50;
            GameManager.instance.slider1.value *= 50;
        }
        if (expenceItem==3 && GameManager.instance.slider1.value < 300)
            GameManager.instance.slider1.value = 300;
        else if(GameManager.instance.slider1.value < 100)
            GameManager.instance.slider1.value = 100;
        GameManager.instance.slider1.transform.GetChild(3).GetComponent<Text>().text = GameManager.instance.slider1.value.ToString();
    }
    public void Task1NextBtn() // next button in dialog box
    {
        if (wrongCatagory && (expenceItem >= 0 && expenceItem <= 3))
        {
            if (PlayerPrefs.GetInt("lives") > 1)
                PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
            GameManager.instance.PopUpShow(1);
            if (PlayerPrefs.GetInt("lives") <= -1)
                GameManager.instance.LifeEnd();            
            return;
        }
        GameManager.instance.CurrentLevelProgress(counter, 6.0f);
        if (counter == 0 && PlayerPrefs.GetInt("grandma") == 2)
        {
            GameManager.instance.DialogPanel.SetActive(true);
            GameManager.instance.DialogueBox.SetActive(true);
            GameManager.instance.DialogueText.text = "Hello Alex, Earning is the first step, but financial responsibility is all about how you spend your money, and that's where everyone makes mistakes.";
            counter++;
        }
        else if (counter == 1)
        {
            GameManager.instance.DialogueText.text = "Let’s start by organizing your virtual wallet. Here are $1700. Spend it wisely.";
            GameManager.instance.wallet.SetActive(true);
            PlayerPrefs.SetInt("walletmoney", 0);
            GameManager.instance.AddMoney(1700);
            counter++;
        }
        else if (counter == 2)
        {
            GameManager.instance.DialogueText.text = "I'll take $700 as rent. You have to organize remaining money.";
            GameManager.instance.RemoveMoney(700);
            counter++;
        }
        else if (counter == 3)
        {
            GameManager.instance.DialogueText.text = "You have to divide your money for groceries, dining out, entertainment and impluse purchases. You have to save atleast $100 or more in order to complete the task.";
            counter++;
        }
        else if (counter == 4)
        {

            GameManager.instance.DialogPanel.transform.GetChild(1).gameObject.SetActive(true);
            GameManager.instance.DialogueBox.SetActive(false);
            Lesson1_1UI.SetActive(true);
            moneycheck = PlayerPrefs.GetInt("walletmoney");
            sliderval = (Int32)GameManager.instance.slider1.value;                                     
                GameManager.instance.RemoveMoney((Int32)GameManager.instance.slider1.value);
            GameManager.instance.slider1.value = 0;
            if (expenceItem >= 0)
                ExpensesArr[expenceItem].SetActive(false);
            if (expenceItem >= 0)
            {
                if (sliderval == ExpensesIdeal[expenceItem ])
                    GameManager.instance.PopUpShow(0);
                else if(sliderval >= ExpensesIdeal[expenceItem]+50 || sliderval <= ExpensesIdeal[expenceItem] - 50)
                {
                     GameManager.instance.PopUpShow(1);
                    if (PlayerPrefs.GetInt("lives") <= -1)
                    {
                        GameManager.instance.LifeEnd();
                        PlayerPrefs.SetInt("grandma", 2);
                        PlayerPrefs.SetInt("gameprogress", 0);
                        return;
                    }
                }
            }
            if (expenceItem < 3)
            {
                wrongCatagory = true;
                expenceItem++;
                ExpensesArr[expenceItem].SetActive(true);
            }
            else
            {                
                if (moneycheck-sliderval < 100)
                {              
                    if (PlayerPrefs.GetInt("walletpoint") >= 5)
                    {
                        GameManager.instance.Toast.Show("Points: -5",3);
                        GameManager.instance.RemovePoint(5);
                    }
                    Lesson1_1UI.SetActive(false);
                    GameManager.instance.DialogueBox.SetActive(true);
                    GameManager.instance.DialogPanel.transform.GetChild(1).gameObject.SetActive(false);
                    GameManager.instance.DialogueText.text = "You couldn't achieve the target. Try spending less money on irresponsible items and save minimum $100. Try again.";
                    
                    GameManager.instance.LivesDeduct();
                    if (PlayerPrefs.GetInt("lives") > -1)
                        Invoke("Retry", 3.5f);
                    else
                    {
                        GameManager.instance.LifeEnd();
                        PlayerPrefs.SetInt("grandma", 2);
                        PlayerPrefs.SetInt("gameprogress", 0);
                    }
                }
                else
                {
                    Lesson1_1UI.SetActive(false);
                    GameManager.instance.DialogueBox.SetActive(true);
                    GameManager.instance.DialogPanel.transform.GetChild(1).gameObject.SetActive(false);
                    GameManager.instance.DialogueText.text = "See how that helps? Once you spend the right amount of money on the right things, you can save money that is all yours to invest. Remember, money is like a magnet. It attracts more money.";
                    counter++;
                }
            }
        }
        else if (counter == 5)
        {
            GameManager.instance.DialogueText.text = "Now, Alex, go off into town and see Mr. Green, I need some seeds and fresh vegetables from his farm.";
            counter++;
        }
        else if (counter == 6)
        {
            GameManager.instance.DialogueBox.SetActive(false);
            PlayerPrefs.SetInt("grandma", 1);
            PlayerPrefs.SetInt("gameprogress", 1);
            GameManager.instance.RewardPanell();
            GameManager.instance.AddMoney(100);
            GameManager.instance.AddPoint(20);
        }
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("grandma", 2);
        PlayerPrefs.SetInt("gameprogress", 0);
        SceneManager.LoadScene(2);
    }
}