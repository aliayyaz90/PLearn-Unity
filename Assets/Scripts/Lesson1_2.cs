using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Lesson1_2 : MonoBehaviour
{
    [SerializeField] public GameObject[] NeedsNWantsArr;
    [SerializeField] public Text MrGreenMoneyText;
    int counter;
    public int shoppingItem;
    int moneycheck,sliderval;
    public int[] shoppingIdeal = { 300, 250, 200, 300, 500 };
    public static bool wrongCatagory2;
    [SerializeField] GameObject Lesso1_2UI;
    void Start()
    {
        wrongCatagory2 = true;
        counter = 0;
        shoppingItem = -1;
        GameManager.instance.AddMoney(0);
        GameManager.instance.AddPoint(0);
        GameManager.instance.wallet.SetActive(true);
        PlayerPrefs.SetInt("mrgreenmoney", 1900);
        GameManager.instance.Toast.Show("Go to Mr.Green", 3);
       // GameManager.toastMsg = "Go to Mr.Green";
        GameManager.instance.LivesCheck();
       // Debug.Log(GameManager.instance.player.localPosition);
        GameManager.instance.NpcImages[1].SetActive(true);
    }
    void Update()
    {
        if (GameManager.instance.slider2.value % 50 != 0)
        {
            GameManager.instance.slider2.value /= 50;
            GameManager.instance.slider2.value *= 50;
        }
        if (shoppingItem == 4 && GameManager.instance.slider2.value < 500)
            GameManager.instance.slider2.value = 500;
        else if (GameManager.instance.slider2.value < 200)
            GameManager.instance.slider2.value = 200;
        GameManager.instance.slider2.transform.GetChild(3).GetComponent<Text>().text = GameManager.instance.slider2.value.ToString();
    }
    public void Task2NextBtn() // next button in dialog box
    {
        //GameManager.toastMsg = "";
        if (wrongCatagory2 && (shoppingItem >= 0 && shoppingItem <= 4))
        {
            if (PlayerPrefs.GetInt("lives") > 1)
                PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
            GameManager.instance.PopUpShow(1);
            if (PlayerPrefs.GetInt("lives") <= -1)
            {
                GameManager.instance.LifeEnd();
               
            }
            return;
        }
        GameManager.instance.CurrentLevelProgress(counter, 4.0f);
        if (counter==0 && PlayerPrefs.GetInt("mrgreen") == 2)
        {
            GameManager.instance.DialogPanel.SetActive(true);
            GameManager.instance.DialogueBox.SetActive(true);
            GameManager.instance.DialogueText.text = "Hello Alex, I'm struggling to budget for my farm. Here is the list of potential purchases.";
            counter++;
        }
        else if (counter == 1)
        {
            GameManager.instance.DialogueText.text = "If I spend too much on things I want instead of what I need, my farm might not make it through the season. You're old Millie's grandson, right? Can you help me figure this out?";
            counter++;
        }
        else if (counter == 2)
        {
            GameManager.instance.DialogueText.text = "You have to sort wants and needs in the shopping list ensurings his $1900 are spent wisely. You have to save atleast $150 or more in order to complete the task.";
            MrGreenMoneyText.gameObject.SetActive(true);
            RemoveMrGreenMoney(0);
            counter++;
        }
        else if (counter == 3)
        {
            GameManager.instance.DialogPanel.transform.GetChild(1).gameObject.SetActive(true);
            GameManager.instance.DialogueBox.SetActive(false);
            Lesso1_2UI.SetActive(true);
            moneycheck = PlayerPrefs.GetInt("mrgreenmoney");
            sliderval = (Int32)GameManager.instance.slider2.value;
                RemoveMrGreenMoney((Int32)GameManager.instance.slider2.value);
            GameManager.instance.slider2.value = 0;
            if (shoppingItem >= 0)
                NeedsNWantsArr[shoppingItem].SetActive(false);
            if (shoppingItem >= 0)
            {
                if (sliderval == shoppingIdeal[shoppingItem])
                    GameManager.instance.PopUpShow(0);
                else if (sliderval >= shoppingIdeal[shoppingItem] + 50 || sliderval <= shoppingIdeal[shoppingItem] - 50)
                {
                    GameManager.instance.PopUpShow(1);
                    if (PlayerPrefs.GetInt("lives") <= -1)
                    {
                        GameManager.instance.LifeEnd();
                        PlayerPrefs.SetInt("mrgreen", 0);
                        PlayerPrefs.SetInt("gameprogress", 1);
                        return;
                    }
                }
            }
            if (shoppingItem < 4)
            {
                wrongCatagory2 = true;
                shoppingItem++;
                NeedsNWantsArr[shoppingItem].SetActive(true);
            }
            else
            {   
                if (moneycheck-sliderval < 150)
                {
                    if (PlayerPrefs.GetInt("walletpoint") >= 5)
                    {
                        GameManager.instance.Toast.Show("Points: -5", 3);
                        GameManager.instance.RemovePoint(5);
                    }
                    Lesso1_2UI.SetActive(false);
                    GameManager.instance.DialogueBox.SetActive(true);
                    GameManager.instance.DialogPanel.transform.GetChild(1).gameObject.SetActive(false);
                    GameManager.instance.DialogueText.text = "You couldn't achieve the target. Try spending less money on wants and save minimum $150. Try again.";
                    //if (PlayerPrefs.GetInt("lives") > 1)
                    //    PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
                    GameManager.instance.LivesDeduct();

                    if (PlayerPrefs.GetInt("lives") > -1)
                        Invoke("Retry", 3.5f);
                    else
                    {
                        GameManager.instance.LifeEnd();
                        PlayerPrefs.SetInt("mrgreen", 0);
                        PlayerPrefs.SetInt("gameprogress", 1);
                    }
                    return;
                }
                else
                {
                    Lesso1_2UI.SetActive(false);
                    GameManager.instance.DialogueBox.SetActive(true);
                    GameManager.instance.DialogPanel.transform.GetChild(1).gameObject.SetActive(false);
                    GameManager.instance.DialogueText.text = "Thankyou Alex for sorting out my expenses. Give these vegitables to Grandma Millie.";
                    counter++;
                }
            }
        }
        else if (counter == 4)
        {
            GameManager.instance.DialogPanel.SetActive(false);
            PlayerPrefs.SetInt("gameprogress", 2);
            PlayerPrefs.SetInt("mrgreen", 1);
            GameManager.instance.RewardPanell();
            GameManager.instance.AddMoney(PlayerPrefs.GetInt("mrgreenmoney"));
            GameManager.instance.AddPoint(20);
        }
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("mrgreen", 0);
        PlayerPrefs.SetInt("gameprogress", 1);
        SceneManager.LoadScene(2);
    }
    private void RemoveMrGreenMoney(int amount)
    {
        if (PlayerPrefs.GetInt("mrgreenmoney") - amount >= 0)
        {
            PlayerPrefs.SetInt("mrgreenmoney", PlayerPrefs.GetInt("mrgreenmoney") - amount);
            MrGreenMoneyText.text="Mr.Green's Budget: $"+ PlayerPrefs.GetInt("mrgreenmoney").ToString();
        }
    }
}