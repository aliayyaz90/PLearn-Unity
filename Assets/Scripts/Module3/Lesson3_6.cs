using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Lesson3_6 : MonoBehaviour
{
    int counter, breakCounter,AccountCount,MCQNo;
    [SerializeField] GameObject Lesson3__6, Task1, Task2,ExplainPanel;
    [SerializeField] Text ExplainHeadingText, ExplainText, timerText;
    [SerializeField] GameObject[] SchumerBox;
    public static bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;
    [SerializeField] Transform task2Popup;
    [SerializeField] Button[] popUpMCQ1;
    public float timer = 90f;
    string[] questions = { "What does \"APR\" stand for, and why is it important?",
                           "If you miss a payment, what penalty might apply?",
                           "What is the \"grace period\" and how can it help you avoid interest?",
                           "Which of these fees is typically found in a Schumer Box?",
                           "How can comparing Schumer Boxes help you choose a better credit card?" };

    [SerializeField] Text questionsText;

    // Start is called before the first frame update
    void Start()
    {
        MCQNo = 0;
        breakCounter = 0;
        AccountCount = 0;
        counter = 0;
        GameManagerModule3.instance.AddMoney(0);
        GameManagerModule3.instance.AddPoint(0);
        GameManagerModule3.instance.AddBankMoney(0, 0);
        GameManagerModule3.instance.LivesCheck();



    }

    public void Update()
    {
        if (counter == 5)
        {
            //PlayerPrefs.SetInt("warn", 0);
            SwipeManager();
            if (timer >= 0)
            {
            timer -= Time.deltaTime;
            timerText.text ="Timer "+((Int32)timer).ToString();
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
    
    public void Task3_6NextBtn() // next button in dialog box
    {
       // Debug.Log(counter);
        GameManagerModule3.instance.CurrentLevelProgress(counter, 5.0f);
        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 6)
        {
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Hey, Alex, can you tell me what this is? They say it's a credit card I've been approved for, but I never applied. Have a look.";
            counter++;
        }
        else if (counter == 1)
        {

            Lesson3__6.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            counter++;
        }
        else if (counter == 2)
        {
            Task1.transform.GetChild(1).gameObject.SetActive(true);
            questionsText.text = questions[0];
            //counter++;
        }
        else if (counter == 3)
        {
            Invoke("DialogueShowWithWait", 2);
            GameManagerModule3.instance.Toast.Show("+10 Points\n+150 Money", 2);
            GameManagerModule3.instance.AddPoint(10);
            GameManagerModule3.instance.AddBankMoney(150,1);
            Task1.SetActive(false);
            GameManagerModule3.instance.DialogueText.text = "Alex, look how many of these there are? And they all feel…odd, please have a look at them.";
            counter++;


        }
        else if (counter == 4)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task2.SetActive(true);
            counter++;
        }

        else if (counter == 5)
        {
            Lesson3__6.SetActive(false);
            PlayerPrefs.SetInt("sofia", 7);
            PlayerPrefs.SetInt("gameprogress", 15);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.AddBankMoney(100, 1);
            GameManagerModule3.instance.AddPoint(15);
            counter++;
        }
    }

    public void InfoBtn(int info)
    {
        ExplainPanel.SetActive(true);
        if (info == 0)
        {
            ExplainHeadingText.text = "APR";
            ExplainText.text = "An APR purchase refers to the Annual Percentage Rate applied to purchases made with a credit card. It represents the yearly interest cost you’ll pay if you carry a balance, not paying your full statement by the due date.";
        }
        else if (info == 1)
        {
            ExplainHeadingText.text = "Fees";
            ExplainText.text = "Credit card fees are charges imposed by the issuer for specific services or actions, like annual maintenance, late payments, or using the card abroad. These fees help cover the cost of offering credit and can vary by card type and usage.";
        }
        else if (info == 2)
        {
            ExplainHeadingText.text = "Grace Period";
            ExplainText.text = "A grace period on a credit card is the time between the end of your billing cycle and your payment due date. During this period, you can pay your full balance without being charged interest on new purchases. If you don’t pay in full, interest starts accruing immediately.";
        }
        else if (info == 3)
        {
            ExplainHeadingText.text = "Penalties";
            ExplainText.text = "Credit card penalties are extra charges or rate increases for not following card terms, like late payments, exceeding your credit limit, or returned payments. These can include penalty fees and higher interest rates, affecting your credit score and overall cost.";
        }
    }

    public void CloseExplainPanel()
    {
        ExplainPanel.SetActive(false);
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
                SchumerBox[i].SetActive(true);
            else
                SchumerBox[i].SetActive(false);

        }
         //Debug.Log(swipeLeft + "left    " + swipeRight + "right");
    }
    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
    public void AccountSelectBtns(int AccountType)
    {
       
        if (AccountType == 2)
        {
            GameManagerModule3.instance.PopUpShow(0);
            Task3_6NextBtn();
        }
        else
        {
            GameManagerModule3.instance.PopUpShow(1);
            if (PlayerPrefs.GetInt("lives") <= -1)
                GameManagerModule3.instance.LifeEnd();
            breakCounter++;
            if (breakCounter >= 3)
                GameManagerModule3.instance.BreakPanel.SetActive(true);
        }
    }


    public void Task1MCQs(int BtnNo)
    {
        if (MCQNo == 0 && BtnNo == 0)
        {
            popUpMCQ1[BtnNo].GetComponent<Image>().color = Color.green;
            GameManagerModule3.instance.PopUpShow(0);
            MCQNo = 1;
            questionsText.text = questions[1];
            timer = 20f;
        }
        else if (MCQNo == 1 && BtnNo == 1)
        {
            popUpMCQ1[BtnNo].GetComponent<Image>().color = Color.green;
            GameManagerModule3.instance.PopUpShow(0);
            MCQNo = 2;
            questionsText.text = questions[2];
            timer = 20f;
        }
        else if (MCQNo == 2 && BtnNo == 2)
        {
            popUpMCQ1[BtnNo].GetComponent<Image>().color = Color.green;
            GameManagerModule3.instance.PopUpShow(0);
            MCQNo = 3;
            questionsText.text = questions[3];
            timer = 20f;
        }
        else if (MCQNo == 3 && BtnNo == 3)
        {
            popUpMCQ1[BtnNo].GetComponent<Image>().color = Color.green;
            GameManagerModule3.instance.PopUpShow(0);
            MCQNo = 4;
            questionsText.text = questions[4];
            timer = 20f;
        }
        else if (MCQNo == 4 && BtnNo == 4)
        {
            popUpMCQ1[BtnNo].GetComponent<Image>().color = Color.green;
            GameManagerModule3.instance.PopUpShow(0);
            counter++;
            Task3_6NextBtn();
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
        popUpMCQ1[0].GetComponent<Image>().color = Color.white;
        popUpMCQ1[1].GetComponent<Image>().color = Color.white;
        popUpMCQ1[2].GetComponent<Image>().color = Color.white;
        popUpMCQ1[3].GetComponent<Image>().color = Color.white;
        popUpMCQ1[4].GetComponent<Image>().color = Color.white;
    }
    public void DialogueShowWithWait()
    {
        GameManagerModule3.instance.NpcImages[2].SetActive(true);
        GameManagerModule3.instance.DialogueBox.SetActive(true);
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 6);
        PlayerPrefs.SetInt("gameprogress", 14);
        SceneManager.LoadScene(4);
    }
}
