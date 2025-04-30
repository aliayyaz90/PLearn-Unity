using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Lesson2_1 : MonoBehaviour
{
    int counter, AccountCount;
    public static bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;
    [SerializeField] GameObject[] Accounts;
    [SerializeField] GameObject Lesson2__1,CreditCardImg;
    [SerializeField] Text AccountNameText, SliderText;
    [SerializeField] Slider slider;
    [SerializeField] InputField[] inputFields;
    int breakCounter;
    void Start()
    {
        AccountCount = 0;
        GameManagerModule2.instance.LivesCheck();
        GameManagerModule2.instance.AddMoney(0);
        GameManagerModule2.instance.AddPoint(0);
        GameManagerModule2.instance.AddBankMoney(0,0);
        slider.maxValue = PlayerPrefs.GetInt("walletmoney");
        PlayerPrefs.SetInt("bankmoney", 0);
        GameManagerModule2.instance.NpcImages[0].SetActive(true);
        GameManagerModule2.instance.NPCsNames.text = "Mr.Patel";
        breakCounter = 0;
    }
    void Update()
    {
        if(counter==7)
            SwipeManager();

        if (slider.value % 50 != 0)
        {
            slider.value /= 50;
            slider.value *= 50;
        }
        if ((AccountCount==0 || AccountCount==2) && slider.value<100)
            slider.value = 100;
        else if(AccountCount == 1 && slider.value < 400)
            slider.value = 400;

        SliderText.text = slider.value.ToString();
    }
    public void Task2_1NextBtn() // next button in dialog box
    {
        GameManagerModule2.instance.CurrentLevelProgress(counter, 10.0f);
       // GameManagerModule2.instance.BankImg.SetActive(true);
        if (counter == 0 && PlayerPrefs.GetInt("patel") == 2)
        {
            GameManagerModule2.instance.DialogPanel.SetActive(true);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            
            GameManagerModule2.instance.DialogueText.text = "Alex, I heard about the good work you did over in Paris, you did great understanding how money should be spent responsibly and within a given limit and parameters, but now it's time to step up your game.";
            counter++;
        }
        else if (counter == 1)
        {
            GameManagerModule2.instance.DialogueText.text = "Money isn’t just about earning and spending, it's also about managing it wisely. It's about knowing the right steps, to keep your money safe, while you spend or earn it. You keep your money safe and secure, through banks, and banks give you accounts that hold money.";
            counter++;
        }
        else if (counter == 2)
        {
            GameManagerModule2.instance.DialogueText.text = " Let’s start with checking accounts, which act as your gateway to safe and convenient banking. Let me explain!";
            counter++;
        }
        else if (counter == 3)
        {
            GameManagerModule2.instance.DialogueText.text = "A checking account helps you deposit, withdraw, and manage money efficiently based on your immediate needs. You'll need one to receive paychecks, pay bills, and avoid carrying cash everywhere.";
            counter++;
        }
        else if (counter == 4)
        {
            GameManagerModule2.instance.DialogueText.text = "It's how money works now; you can't carry large sums in cash, which is both inconvenient and unsafe. Before we set you out on your banking journey, let's get you started on that account!";
            counter++;
        }
        else if (counter == 5)
        {
            GameManagerModule2.instance.DialogueText.text = "Choose one account from the three accounts according to your needs.";
            counter++;
        }
        else if (counter == 6)
        {
            GameManagerModule2.instance.NpcImages[0].SetActive(false);
            Lesson2__1.SetActive(true);
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            counter++;
            GameManagerModule2.instance.PopUpShow(2);
        }
        else if(counter == 7)
        {
            Lesson2__1.transform.GetChild(0).gameObject.SetActive(false);
            Lesson2__1.transform.GetChild(1).gameObject.SetActive(true);
            if (AccountCount == 0)
                AccountNameText.text = "Basic Account";
            else if (AccountCount == 1)
                AccountNameText.text = "Premium Account";
            else if(AccountCount == 2)
                AccountNameText.text = "Student Account";
            counter++;
        }
        else if (counter == 8)
        {
            Lesson2__1.transform.GetChild(1).gameObject.SetActive(false);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.NpcImages[0].SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "Congratulations, welcome to modern banking Alex! Your checking account will make financial transactions seamless and easy, but it will also mean all your money is safe and sound. Now, let’s learn more about banks and see how banks compare to other financial institutions.";
            counter++;
        }
        else if (counter == 9)
        {
            GameManagerModule2.instance.DialogueText.text = "Here is your virtual debit card, Alex.";
            CreditCardImg.SetActive(true);
            counter++;
        }
        else if (counter == 10)
        {
            Lesson1_4.AnimationLevel = false;
            GameManagerModule2.instance.NpcImages[0].SetActive(false);
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            PlayerPrefs.SetInt("patel", 1);
            PlayerPrefs.SetInt("gameprogress", 5);
            GameManagerModule2.instance.RewardPanell();
            GameManagerModule2.instance.AddBankMoney(100,1);
            GameManagerModule2.instance.AddPoint(10);         
        }
    }
    public void GoBackBtn()
    {
        counter = 7;
        Lesson2__1.transform.GetChild(1).gameObject.SetActive(false);
        Lesson2__1.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void SwipeManager()
    {
        tap = swipeDown = swipeUp = swipeLeft = swipeRight = false;


        #region Mobile Input
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                Reset();
            }

        }
        #endregion

        //Calculate the distance
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length < 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

        //Did we cross the distance?
        if (swipeDelta.magnitude > 100)
        {
            //Which direction?
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Left or Right
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                //Up or Down
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }

            Reset();
        }
        if (swipeLeft)
        {
            AccountCount++;
            if (AccountCount == 3)
                AccountCount = 0;
        }
        else if (swipeRight)
        {
            AccountCount--;
            if (AccountCount == -1)
                AccountCount = 2;
        }
        for (int i = 0; i < 3; i++)
        {
            if (i == AccountCount)
                Accounts[i].SetActive(true);
            else
                Accounts[i].SetActive(false);

        }
       // Debug.Log(swipeLeft + "left    " + swipeRight + "right");
    }
    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
    public void AccountSelectBtns(int AccountType)
    {
        Task2_1NextBtn();
        if (AccountType == 1)
            GameManagerModule2.instance.PopUpShow(0);
        else
        {
            GameManagerModule2.instance.PopUpShow(4);
            breakCounter++;
            if (breakCounter >= 3)
                GameManagerModule2.instance.BreakPanel.SetActive(true);            
        }
    }   
    public void Retry()
    {
        PlayerPrefs.SetInt("gameprogress", 4);
        PlayerPrefs.SetInt("patel", 0);
        SceneManager.LoadScene(3);    
    }
    public void VirtualAppSubmitBtn()
    {
        for (int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i].text == "")
            {
                GameManagerModule2.instance.PopUpShow(3);
                breakCounter++;
                if (breakCounter >= 3)
                    GameManagerModule2.instance.BreakPanel.SetActive(true);
                return;                 
            }
        }
        for (int j = 0; j < inputFields[0].text.Length; j++)
        {
            int inp;
            if (int.TryParse(inputFields[0].text.Substring(j), out inp))
            {
                GameManagerModule2.instance.PopUpShow(3);
                breakCounter++;
                if (breakCounter >= 3)
                    GameManagerModule2.instance.BreakPanel.SetActive(true);
                return;
            }
        }
        if (((AccountCount == 0 || AccountCount == 2) && slider.value < 100) || AccountCount==1 && slider.value < 400)
        {
            GameManagerModule2.instance.PopUpShow(3);
            breakCounter++;
            if (breakCounter >= 3)
                GameManagerModule2.instance.BreakPanel.SetActive(true);
            return;
        }
        GameManagerModule2.instance.wallet.transform.GetChild(2).gameObject.SetActive(true);
        GameManagerModule2.instance.AddBankMoney((Int32)slider.value,0);
        GameManagerModule2.instance.RemoveMoney((Int32)slider.value);
        Task2_1NextBtn();
    }
}